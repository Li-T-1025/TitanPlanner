
namespace GMap.NET.MapProviders
{
    using GMap.NET.Projections;
    using System;

   /// <summary>
   /// GoogleChinaSatelliteMap provider - 国内镜像 ggmap01.bosim.top:8880
   /// </summary>
   public class GoogleChinaSatelliteMapProvider : GoogleMapProviderBase
   {
      public static readonly GoogleChinaSatelliteMapProvider Instance;

      GoogleChinaSatelliteMapProvider()
      {
         RefererUrl = "http://ggmap01.bosim.top:8880/";
      }

      static GoogleChinaSatelliteMapProvider()
      {
         Instance = new GoogleChinaSatelliteMapProvider();
      }

      public override PureProjection Projection
      {
          get
          {
              return MercatorProjectionGCJ.Instance;
          }
      }

      public string Version = "s@165";

      #region GMapProvider Members

      readonly Guid id = new Guid("543009AC-3379-4893-B580-DBE6372B1753");
      public override Guid Id
      {
         get
         {
            return id;
         }
      }

      readonly string name = "Google 卫星地图";
      public override string Name
      {
         get
         {
            return name;
         }
      }

      public override PureImage GetTileImage(GPoint pos, int zoom)
      {
         string url = string.Format("http://ggmap01.bosim.top:8880/vt/lyrs=s&hl=zh-CN&gl=CN&src=app&x={0}&y={1}&z={2}", pos.X, pos.Y, zoom);
         try
         {
             using (var wc = new System.Net.WebClient())
             {
                 wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                 wc.Headers.Add("Referer", "http://ggmap01.bosim.top:8880/");
                 byte[] data = wc.DownloadData(url);
                 return TileImageProxy.FromArray(data);
             }
         }
         catch { return null; }
      }

      #endregion
   }
}
