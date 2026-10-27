using System;
using GMap.NET.Projections;

namespace GMap.NET.MapProviders
{
    /// <summary>
    /// 天地图 Provider 基类
    /// 申请 tk: https://console.tianditu.gov.cn/
    /// </summary>
    public abstract class TianDiTuMapProviderBase : GMapProvider
    {
        // ====== 从 Settings 读取 tk，未配置时为空 ======
        public static string TianDiTuKey = "";
        // ====================================

        public TianDiTuMapProviderBase()
        {
            MaxZoom = 18;
            RefererUrl = "https://www.tianditu.gov.cn/";
            Copyright = "© 天地图 国家地理信息公共服务平台";
        }

        protected string GetUrl(string layer, GPoint pos, int zoom)
        {
            // 未配置 tk 时不加载
            if (string.IsNullOrEmpty(TianDiTuKey))
                return string.Empty;
            // t0-t7 子域名轮询
            int server = (int)((pos.X + pos.Y) % 8);
            return string.Format(
                "https://t{0}.tianditu.gov.cn/DataServer?T={1}&x={2}&y={3}&l={4}&tk={5}",
                server, layer, pos.X, pos.Y, zoom, TianDiTuKey);
        }

        public override PureProjection Projection
        {
            get { return MercatorProjection.Instance; }
        }

        public override GMapProvider[] Overlays
        {
            get { return new GMapProvider[] { this }; }
        }
    }

    /// <summary>
    /// 天地图 矢量地图（含注记）
    /// </summary>
    public class TianDiTuVectorMapProvider : TianDiTuMapProviderBase
    {
        public static readonly TianDiTuVectorMapProvider Instance;

        static TianDiTuVectorMapProvider()
        {
            Instance = new TianDiTuVectorMapProvider();
        }

        private TianDiTuVectorMapProvider() { }

        readonly Guid id = new Guid("A1B2C3D4-E5F6-7890-ABCD-EF1234567890");
        public override Guid Id
        {
            get { return id; }
        }

        public override string Name
        {
            get { return "天地图-矢量地图"; }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = GetUrl("vec_w", pos, zoom);
            try
            {
                using (var wc = new System.Net.WebClient())
                {
                    wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                    byte[] data = wc.DownloadData(url);
                    return TileImageProxy.FromArray(data);
                }
            }
            catch { return null; }
        }
    }

    /// <summary>
    /// 天地图 矢量注记层
    /// </summary>
    public class TianDiTuVectorLabelProvider : TianDiTuMapProviderBase
    {
        public static readonly TianDiTuVectorLabelProvider Instance;

        static TianDiTuVectorLabelProvider()
        {
            Instance = new TianDiTuVectorLabelProvider();
        }

        private TianDiTuVectorLabelProvider() { }

        readonly Guid id = new Guid("B2C3D4E5-F6A7-8901-BCDE-F12345678901");
        public override Guid Id
        {
            get { return id; }
        }

        public override string Name
        {
            get { return "天地图-矢量注记"; }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = GetUrl("cva_w", pos, zoom);
            try
            {
                using (var wc = new System.Net.WebClient())
                {
                    wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                    byte[] data = wc.DownloadData(url);
                    return TileImageProxy.FromArray(data);
                }
            }
            catch { return null; }
        }
    }

    /// <summary>
    /// 天地图 影像地图（卫星图）
    /// </summary>
    public class TianDiTuSatelliteMapProvider : TianDiTuMapProviderBase
    {
        public static readonly TianDiTuSatelliteMapProvider Instance;

        static TianDiTuSatelliteMapProvider()
        {
            Instance = new TianDiTuSatelliteMapProvider();
        }

        private TianDiTuSatelliteMapProvider() { }

        readonly Guid id = new Guid("C3D4E5F6-A7B8-9012-CDEF-123456789012");
        public override Guid Id
        {
            get { return id; }
        }

        public override string Name
        {
            get { return "天地图-卫星影像"; }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = GetUrl("img_w", pos, zoom);
            try
            {
                using (var wc = new System.Net.WebClient())
                {
                    wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                    byte[] data = wc.DownloadData(url);
                    return TileImageProxy.FromArray(data);
                }
            }
            catch { return null; }
        }
    }

    /// <summary>
    /// 天地图 影像注记层
    /// </summary>
    public class TianDiTuSatelliteLabelProvider : TianDiTuMapProviderBase
    {
        public static readonly TianDiTuSatelliteLabelProvider Instance;

        static TianDiTuSatelliteLabelProvider()
        {
            Instance = new TianDiTuSatelliteLabelProvider();
        }

        private TianDiTuSatelliteLabelProvider() { }

        readonly Guid id = new Guid("D4E5F6A7-B8C9-0123-DEFA-234567890123");
        public override Guid Id
        {
            get { return id; }
        }

        public override string Name
        {
            get { return "天地图-影像注记"; }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = GetUrl("cia_w", pos, zoom);
            try
            {
                using (var wc = new System.Net.WebClient())
                {
                    wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                    byte[] data = wc.DownloadData(url);
                    return TileImageProxy.FromArray(data);
                }
            }
            catch { return null; }
        }
    }

    /// <summary>
    /// 天地图 混合地图（影像+注记）
    /// </summary>
    public class TianDiTuHybridMapProvider : TianDiTuMapProviderBase
    {
        public static readonly TianDiTuHybridMapProvider Instance;

        static TianDiTuHybridMapProvider()
        {
            Instance = new TianDiTuHybridMapProvider();
        }

        private TianDiTuHybridMapProvider() { }

        readonly Guid id = new Guid("E5F6A7B8-C9D0-1234-EFAB-345678901234");
        public override Guid Id
        {
            get { return id; }
        }

        public override string Name
        {
            get { return "天地图-卫星混合"; }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            return TianDiTuSatelliteMapProvider.Instance.GetTileImage(pos, zoom);
        }

        public override GMapProvider[] Overlays
        {
            get
            {
                return new GMapProvider[] {
                    TianDiTuSatelliteMapProvider.Instance,
                    TianDiTuSatelliteLabelProvider.Instance
                };
            }
        }
    }

    /// <summary>
    /// 天地图 地形地图
    /// </summary>
    public class TianDiTuTerrainMapProvider : TianDiTuMapProviderBase
    {
        public static readonly TianDiTuTerrainMapProvider Instance;

        static TianDiTuTerrainMapProvider()
        {
            Instance = new TianDiTuTerrainMapProvider();
        }

        private TianDiTuTerrainMapProvider() { }

        readonly Guid id = new Guid("F6A7B8C9-D0E1-2345-FABC-456789012345");
        public override Guid Id
        {
            get { return id; }
        }

        public override string Name
        {
            get { return "天地图-地形"; }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = GetUrl("ter_w", pos, zoom);
            try
            {
                using (var wc = new System.Net.WebClient())
                {
                    wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                    byte[] data = wc.DownloadData(url);
                    return TileImageProxy.FromArray(data);
                }
            }
            catch { return null; }
        }
    }
}