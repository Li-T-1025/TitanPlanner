using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using OpenTK;
using OpenTK.Graphics.ES20;

namespace MissionPlanner.Controls.Scene3D
{
    /// <summary>
    /// Frame rate and terrain streaming counters in the bottom-left corner. The text texture is only
    /// re-rendered when the text changes (about once a second).
    /// </summary>
    public sealed class StatsOverlay : IDisposable
    {
        const int Padding = 8;

        readonly Stopwatch _watch = Stopwatch.StartNew();
        readonly QuadRenderer _quads = new QuadRenderer();
        int _frames;
        double _fps;
        string _text = "";
        int _texture;
        int _width;
        int _height;

        public void Draw(int viewportWidth, int viewportHeight, TerrainEngine terrain)
        {
            _frames++;
            if (_watch.ElapsedMilliseconds >= 1000 || _texture == 0)
            {
                _fps = _frames / Math.Max(0.001, _watch.ElapsedMilliseconds / 1000.0);
                _frames = 0;
                _watch.Restart();

                string text = string.Format("FPS: {0:F0}   Tiles: {1}", _fps, terrain.DrawnTiles);
                int loading = terrain.LoadingTiles;
                if (loading > 0)
                    text += "   Loading: " + loading;
                if (terrain.FailedTiles > 0)
                {
                    string error = terrain.LastError ?? "";
                    text += "   Failed: " + terrain.FailedTiles +
                            (error.Length > 0 ? " (" + (error.Length > 60 ? error.Substring(0, 60) : error) + ")" : "");
                }

                if (text != _text || _texture == 0)
                {
                    _text = text;
                    RenderText();
                }
            }

            if (_texture == 0 || viewportWidth <= 0 || viewportHeight <= 0)
                return;

            var projection = Matrix4.CreateOrthographicOffCenter(0, viewportWidth, viewportHeight, 0, -1, 1);
            var identity = Matrix4.Identity;
            double left = Padding, right = Padding + _width;
            double bottom = viewportHeight - Padding, top = bottom - _height;

            GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            _quads.Begin();
            _quads.Add(_texture, new Vector3d(left, top, 0), new Vector3d(right, top, 0),
                new Vector3d(left, bottom, 0), new Vector3d(right, bottom, 0));
            _quads.Draw(ref projection, ref identity);
        }

        void RenderText()
        {
            using (var font = new Font("Segoe UI", 10, FontStyle.Bold, GraphicsUnit.Point))
            {
                Size size;
                using (var measure = new Bitmap(1, 1))
                using (var g = Graphics.FromImage(measure))
                {
                    var measured = g.MeasureString(_text, font);
                    size = new Size((int)Math.Ceiling(measured.Width + 8), (int)Math.Ceiling(measured.Height + 4));
                }

                using (var bitmap = new Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                {
                    using (var g = Graphics.FromImage(bitmap))
                    using (var background = new SolidBrush(Color.FromArgb(160, 0, 0, 0)))
                    {
                        g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                        g.Clear(Color.Transparent);
                        g.FillRectangle(background, 0, 0, size.Width, size.Height);
                        g.DrawString(_text, font, Brushes.White, new PointF(4, 2));
                    }

                    GLTextures.Delete(ref _texture);
                    _texture = GLTextures.CreateFromImage(bitmap, false);
                    _width = size.Width;
                    _height = size.Height;
                }
            }
        }

        public void Dispose()
        {
            GLTextures.Delete(ref _texture);
            _quads.Dispose();
        }
    }
}
