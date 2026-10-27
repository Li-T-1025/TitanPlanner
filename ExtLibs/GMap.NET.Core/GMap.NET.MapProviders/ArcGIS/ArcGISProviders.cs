namespace GMap.NET.MapProviders
{
    using GMap.NET;
    using GMap.NET.Projections;
    using System;

    /// <summary>
    /// ArcGIS (Esri) 地图基类
    /// </summary>
    public abstract class ArcGISMapProviderBase : GMapProvider
    {
        public ArcGISMapProviderBase()
        {
            MaxZoom = 19;
            RefererUrl = "https://www.arcgis.com/";
            Copyright = "© Esri";
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
    /// ArcGIS 卫星影像图 (Esri World Imagery)
    /// </summary>
    public class ArcGISSatelliteProvider : ArcGISMapProviderBase
    {
        public static readonly ArcGISSatelliteProvider Instance;

        static ArcGISSatelliteProvider()
        {
            Instance = new ArcGISSatelliteProvider();
        }

        private ArcGISSatelliteProvider() { }

        readonly Guid id = new Guid("A7B3C2D1-E4F5-4657-89AB-CDEF01234567");
        public override Guid Id
        {
            get { return id; }
        }

        public override string Name
        {
            get { return "ArcGIS 卫星地图"; }
        }

        // ArcGIS tile URL: /tile/{z}/{y}/{x} （注意顺序是 z,y,x）
        static readonly string UrlFormat = "https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{0}/{1}/{2}";

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = string.Format(UrlFormat, zoom, pos.Y, pos.X);
            return GetTileImageUsingHttp(url);
        }
    }

    /// <summary>
    /// ArcGIS 地名标注层（透明PNG，用于叠加在卫星图上）
    /// </summary>
    public class ArcGISLabelsProvider : ArcGISMapProviderBase
    {
        public static readonly ArcGISLabelsProvider Instance;

        static ArcGISLabelsProvider()
        {
            Instance = new ArcGISLabelsProvider();
        }

        private ArcGISLabelsProvider() { }

        readonly Guid id = new Guid("B8C4D3E2-F5A6-4768-9ABC-DEF012345678");
        public override Guid Id
        {
            get { return id; }
        }

        public override string Name
        {
            get { return "ArcGIS 标注"; }
        }

        static readonly string UrlFormat = "https://server.arcgisonline.com/ArcGIS/rest/services/Reference/World_Boundaries_and_Places/MapServer/tile/{0}/{1}/{2}";

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = string.Format(UrlFormat, zoom, pos.Y, pos.X);
            return GetTileImageUsingHttp(url);
        }
    }

    /// <summary>
    /// ArcGIS 混合地图（卫星影像 + 地名标注）
    /// </summary>
    public class ArcGISHybridProvider : ArcGISMapProviderBase
    {
        public static readonly ArcGISHybridProvider Instance;

        static ArcGISHybridProvider()
        {
            Instance = new ArcGISHybridProvider();
        }

        private ArcGISHybridProvider() { }

        readonly Guid id = new Guid("C9D5E4F3-A6B7-4879-BCDE-F0123456789A");
        public override Guid Id
        {
            get { return id; }
        }

        public override string Name
        {
            get { return "ArcGIS 混合地图"; }
        }

        GMapProvider[] overlays;
        public override GMapProvider[] Overlays
        {
            get
            {
                if (overlays == null)
                {
                    overlays = new GMapProvider[] { ArcGISSatelliteProvider.Instance, ArcGISLabelsProvider.Instance };
                }
                return overlays;
            }
        }

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            return ArcGISSatelliteProvider.Instance.GetTileImage(pos, zoom);
        }
    }

    /// <summary>
    /// ArcGIS 矢量道路图 (Esri World Street Map)
    /// </summary>
    public class ArcGISStreetProvider : ArcGISMapProviderBase
    {
        public static readonly ArcGISStreetProvider Instance;

        static ArcGISStreetProvider()
        {
            Instance = new ArcGISStreetProvider();
        }

        private ArcGISStreetProvider() { }

        readonly Guid id = new Guid("D0E6F5A4-B7C8-498A-CDEF-0123456789AB");
        public override Guid Id
        {
            get { return id; }
        }

        public override string Name
        {
            get { return "ArcGIS 道路地图"; }
        }

        static readonly string UrlFormat = "https://server.arcgisonline.com/ArcGIS/rest/services/World_Street_Map/MapServer/tile/{0}/{1}/{2}";

        public override PureImage GetTileImage(GPoint pos, int zoom)
        {
            string url = string.Format(UrlFormat, zoom, pos.Y, pos.X);
            return GetTileImageUsingHttp(url);
        }
    }
}
