namespace GMap.NET.MapProviders
{
    using GMap.NET;
    using System;

    /// <summary>
    /// 高德混合地图（卫星影像 + 路网标注）
    /// </summary>
    public class AMapHybridProvider : AMapProviderBase
    {
        public static readonly AMapHybridProvider Instance;

        static AMapHybridProvider()
        {
            Instance = new AMapHybridProvider();
        }

        private AMapHybridProvider() { }

        readonly Guid id = new Guid("FCA94AF4-3467-47c6-BDA2-6F52E4A145BD");
        public override Guid Id
        {
            get { return id; }
        }

        public override string Name
        {
            get { return "高德 混合地图"; }
        }

        GMapProvider[] overlays;
        public override GMapProvider[] Overlays
        {
            get
            {
                if (overlays == null)
                {
                    overlays = new GMapProvider[] { AMapSateliteProvider.Instance, this };
                }
                return overlays;
            }
        }

        // 本层只返回路网标注（透明PNG），叠在卫星图上
        static readonly string UrlFormat = "http://webst0{0}.is.autonavi.com/appmaptile?x={1}&y={2}&z={3}&lang=zh_cn&size=1&scale=1&style=8";

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            int server = (int)((pos.X + pos.Y) % 4) + 1;
            string url = string.Format(UrlFormat, server, pos.X, pos.Y, zoom);
            return base.GetTileImageUsingHttp(url);
        }
    }
}
