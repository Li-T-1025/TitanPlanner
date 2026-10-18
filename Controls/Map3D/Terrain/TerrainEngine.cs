using System;
using System.Collections.Generic;
using System.Diagnostics;
using log4net;
using OpenTK;
using OpenTK.Graphics.ES20;

namespace MissionPlanner.Controls.Scene3D
{
    /// <summary>
    /// Streams satellite-textured terrain: selects tiles with <see cref="TerrainQuadtree"/>, loads them with
    /// <see cref="TileLoader"/>, uploads finished tiles within a per-frame time budget, keeps a GPU cache
    /// bounded by memory, and draws with distance fog. All methods run on the render thread with the GL
    /// context current.
    /// </summary>
    public sealed class TerrainEngine : IDisposable
    {
        /// <summary>Time per frame spent uploading finished tiles, so a burst of arrivals never stalls a frame.</summary>
        const double UploadBudgetMs = 4;

        const long GpuBudgetBytes = 384L * 1024 * 1024;

        /// <summary>Forget idle, unloaded tiles after this many frames (about half a minute).</summary>
        const int PruneAfterFrames = 2000;

        static readonly long RemeshRetryTicks = TimeSpan.FromSeconds(2).Ticks;
        static readonly long NoDataRetryTicks = TimeSpan.FromMinutes(2).Ticks;

        static readonly ILog log = LogManager.GetLogger(typeof(TerrainEngine));
        int _loggedOutcomes;

        readonly WorldFrame _frame;
        readonly TerrainQuadtree _quadtree;
        readonly TileLoader _loader;
        readonly Dictionary<int, int> _indexBuffers = new Dictionary<int, int>();
        readonly List<TerrainTile> _evictionCandidates = new List<TerrainTile>();
        readonly List<TileId> _pruned = new List<TileId>();
        readonly Stopwatch _uploadTimer = new Stopwatch();
        GLProgram _program;
        long _gpuBytes;
        int _zoneVersion = -1;

        public TerrainEngine(WorldFrame frame, ITileImagerySource imagery)
        {
            _frame = frame;
            _quadtree = new TerrainQuadtree(frame);
            _loader = new TileLoader(imagery);
        }

        public int DrawnTiles
        {
            get { return _quadtree.DrawList.Count; }
        }

        public int ResidentTiles { get; private set; }

        /// <summary>Tiles whose last load failed (network or decode errors), waiting to retry.</summary>
        public int FailedTiles { get; private set; }

        public string LastError { get; private set; }

        public int LoadingTiles
        {
            get { return _loader.PendingCount + _loader.InFlightCount; }
        }

        /// <summary>
        /// Uploads finished tiles, then chooses what to draw and what to load next for this view.
        /// </summary>
        public void Update(SelectionParams view)
        {
            if (_frame.ZoneVersion != _zoneVersion)
            {
                // Built geometry belongs to the old zone.
                ReleaseAll();
                _zoneVersion = _frame.ZoneVersion;
            }

            long now = DateTime.UtcNow.Ticks;
            UploadResults(now);

            _quadtree.Select(view, now);
            _loader.SetRequests(_quadtree.Requests);

            EvictOverBudget();
            if (_quadtree.Frame % 120 == 0)
                _quadtree.PruneStale(PruneAfterFrames, now, _pruned);
        }

        /// <summary>Stops loading without dropping the GPU cache, e.g. while the view is hidden.</summary>
        public void Pause()
        {
            _loader.SetRequests(new TileRequest[0]);
        }

        void UploadResults(long now)
        {
            _uploadTimer.Restart();
            TileLoadResult result;
            while (_uploadTimer.Elapsed.TotalMilliseconds < UploadBudgetMs && _loader.TryGetResult(out result))
            {
                if (result.ZoneVersion != _zoneVersion)
                    continue;

                var tile = _quadtree.GetOrCreate(result.Id);
                switch (result.Outcome)
                {
                    case TileLoadOutcome.Loaded:
                        Upload(tile, result);
                        break;

                    case TileLoadOutcome.StillPending:
                        tile.RemeshAtTicks = now + RemeshRetryTicks;
                        break;

                    case TileLoadOutcome.NoData:
                        // Usually permanent (no imagery at this zoom), but a provider can also 404 while its tile
                        // version is being looked up, so check again now and then.
                        Log(result);
                        tile.State = TileState.NoData;
                        tile.RetryAtTicks = now + NoDataRetryTicks;
                        break;

                    case TileLoadOutcome.Offline:
                        if (tile.State != TileState.Ready)
                        {
                            tile.State = TileState.Unavailable;
                            tile.RetryAtTicks = now + TimeSpan.FromSeconds(30).Ticks;
                        }

                        break;

                    default:
                        Log(result);
                        LastError = result.Error;
                        if (tile.State == TileState.Ready)
                        {
                            // A remesh failed; keep the tile and try again later.
                            tile.RemeshAtTicks = now + RemeshRetryTicks;
                        }
                        else
                        {
                            tile.State = TileState.Failed;
                            tile.FailCount++;
                            int seconds = Math.Min(60, 2 << Math.Min(5, tile.FailCount));
                            tile.RetryAtTicks = now + TimeSpan.FromSeconds(seconds).Ticks;
                        }

                        break;
                }
            }
        }

        void Log(TileLoadResult result)
        {
            // Enough to diagnose a provider or network problem without flooding the log.
            if (_loggedOutcomes++ < 50)
                log.Info("Tile " + result.Id + " " + result.Outcome + (result.Error != null ? ": " + result.Error : ""));
        }

        void Upload(TerrainTile tile, TileLoadResult result)
        {
            var mesh = result.Mesh;

            if (result.Pixels != null)
            {
                GLTextures.Delete(ref tile.TextureId);
                tile.TextureId = GLTextures.CreateFromBgra(result.Pixels, result.Width, result.Height, true);
            }

            if (tile.TextureId == 0)
            {
                // A remesh for a tile whose texture was evicted meanwhile: load it again from scratch.
                ReleaseGpu(tile);
                return;
            }

            if (tile.VertexBufferId == 0)
                GL.GenBuffers(1, out tile.VertexBufferId);
            GL.BindBuffer(BufferTarget.ArrayBuffer, tile.VertexBufferId);
            GL.BufferData(BufferTarget.ArrayBuffer, mesh.Vertices.Length * sizeof(float), mesh.Vertices,
                BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            _gpuBytes -= tile.GpuBytes;
            int textureBytes = result.Pixels != null ? result.Width * result.Height * 4 * 4 / 3 : TextureBytes(tile);
            tile.GpuBytes = textureBytes + mesh.Vertices.Length * sizeof(float);
            _gpuBytes += tile.GpuBytes;

            tile.GridN = mesh.GridN;
            tile.Heights = mesh.Heights;
            tile.HeightsKnown = true;
            tile.MinHeight = mesh.MinHeight;
            tile.MaxHeight = mesh.MaxHeight;
            tile.HeightsAreFallback = result.HeightsAreFallback;
            tile.RemeshAtTicks = 0;
            tile.OriginEasting = mesh.OriginEasting;
            tile.OriginNorthing = mesh.OriginNorthing;
            tile.MeshZoneVersion = result.ZoneVersion;
            tile.MinEasting = mesh.MinEasting;
            tile.MaxEasting = mesh.MaxEasting;
            tile.MinNorthing = mesh.MinNorthing;
            tile.MaxNorthing = mesh.MaxNorthing;
            tile.BoundsZoneVersion = result.ZoneVersion;
            tile.FailCount = 0;
            tile.State = TileState.Ready;
        }

        static int TextureBytes(TerrainTile tile)
        {
            return (int)Math.Max(0, tile.GpuBytes - TileMeshBuilder.VertexCount(tile.GridN) * TileMeshBuilder.FloatsPerVertex * sizeof(float));
        }

        void EvictOverBudget()
        {
            ResidentTiles = 0;
            FailedTiles = 0;
            _evictionCandidates.Clear();
            foreach (var tile in _quadtree.Tiles)
            {
                if (tile.State == TileState.Failed)
                    FailedTiles++;
                if (tile.State != TileState.Ready)
                    continue;
                ResidentTiles++;
                if (tile.LastVisitedFrame != _quadtree.Frame)
                    _evictionCandidates.Add(tile);
            }

            if (_gpuBytes <= GpuBudgetBytes)
                return;

            _evictionCandidates.Sort((a, b) => a.LastVisitedFrame.CompareTo(b.LastVisitedFrame));
            long target = GpuBudgetBytes * 9 / 10;
            foreach (var tile in _evictionCandidates)
            {
                if (_gpuBytes <= target)
                    break;
                ReleaseGpu(tile);
                ResidentTiles--;
            }
        }

        void ReleaseGpu(TerrainTile tile)
        {
            GLTextures.Delete(ref tile.TextureId);
            if (tile.VertexBufferId != 0)
            {
                GL.DeleteBuffers(1, ref tile.VertexBufferId);
                tile.VertexBufferId = 0;
            }

            _gpuBytes -= tile.GpuBytes;
            tile.GpuBytes = 0;
            if (tile.State == TileState.Ready)
                tile.State = TileState.Unloaded;
        }

        void ReleaseAll()
        {
            foreach (var tile in _quadtree.Tiles)
                ReleaseGpu(tile);
            _quadtree.Clear();
            _gpuBytes = 0;
        }

        int IndexBuffer(int gridN)
        {
            int id;
            if (_indexBuffers.TryGetValue(gridN, out id))
                return id;

            var indices = TileMeshBuilder.BuildIndices(gridN);
            GL.GenBuffers(1, out id);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, id);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(ushort), indices,
                BufferUsageHint.StaticDraw);
            _indexBuffers[gridN] = id;
            return id;
        }

        /// <summary>
        /// Haze starts a third of the way out and is opaque just inside the view radius. Terrain is selected
        /// all the way to the radius, so its edge is always hidden in the fog.
        /// </summary>
        public static void FogRange(double viewRadius, out float start, out float end)
        {
            start = (float)(viewRadius * 0.35);
            end = (float)(viewRadius * 0.95);
        }

        /// <summary>
        /// Draws the selected tiles. Fog reaches the horizon colour just inside the view radius, so the edge
        /// of the loaded terrain is never visible.
        /// </summary>
        public void Draw(ref Matrix4 projection, ref Matrix4 view, double viewRadius, float[] fogColor)
        {
            var tiles = _quadtree.DrawList;
            if (tiles.Count == 0)
                return;

            if (_program == null)
                _program = GLProgram.Create(Shaders.TerrainVertex, Shaders.TerrainFragment);

            float fogStart, fogEnd;
            FogRange(viewRadius, out fogStart, out fogEnd);

            _program.Use();
            _program.SetMatrix("Projection", ref projection);
            _program.SetFloat("FogStart", fogStart);
            _program.SetFloat("FogEnd", fogEnd);
            _program.SetColor("FogColor", fogColor[0], fogColor[1], fogColor[2], 1);
            _program.SetInt("Texture", 0);
            GL.ActiveTexture(TextureUnit.Texture0);

            int position = _program.Attrib("Position");
            int texCoord = _program.Attrib("TexCoord");
            int modelView = _program.Uniform("ModelView");
            int stride = TileMeshBuilder.FloatsPerVertex * sizeof(float);
            GL.EnableVertexAttribArray(position);
            GL.EnableVertexAttribArray(texCoord);

            int boundIndices = -1;
            foreach (var tile in tiles)
            {
                if (tile.VertexBufferId == 0 || tile.TextureId == 0)
                    continue;

                int indices = IndexBuffer(tile.GridN);
                if (indices != boundIndices)
                {
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, indices);
                    boundIndices = indices;
                }

                var model = Matrix4.CreateTranslation((float)(tile.OriginEasting - _frame.OriginEasting),
                    (float)(tile.OriginNorthing - _frame.OriginNorthing), 0);
                var mv = Matrix4.Mult(model, view);
                GL.UniformMatrix4(modelView, 1, false, ref mv.Row0.X);

                GL.BindTexture(TextureTarget.Texture2D, tile.TextureId);
                GL.BindBuffer(BufferTarget.ArrayBuffer, tile.VertexBufferId);
                GL.VertexAttribPointer(position, 3, VertexAttribPointerType.Float, false, stride, IntPtr.Zero);
                GL.VertexAttribPointer(texCoord, 2, VertexAttribPointerType.Float, false, stride, (IntPtr)(3 * sizeof(float)));
                GL.DrawElements(PrimitiveType.Triangles, TileMeshBuilder.IndexCount(tile.GridN),
                    DrawElementsType.UnsignedShort, IntPtr.Zero);
            }

            GL.DisableVertexAttribArray(position);
            GL.DisableVertexAttribArray(texCoord);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public void Dispose()
        {
            _loader.Dispose();
            try
            {
                ReleaseAll();
                foreach (var id in _indexBuffers.Values)
                {
                    int buffer = id;
                    GL.DeleteBuffers(1, ref buffer);
                }

                _indexBuffers.Clear();
                _program?.Dispose();
                _program = null;
            }
            catch
            {
                // The context may already be gone during shutdown.
            }
        }
    }
}
