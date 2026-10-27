
namespace GMap.NET.MapProviders
{
    using GMap.NET.Projections;
    using System;

   /// <summary>
   /// GoogleChinaMap provider - 国内镜像 ggmap01.bosim.top:8880
   /// </summary>
   public class GoogleChinaMapProvider : GoogleMapProviderBase
   {
      public static readonly GoogleChinaMapProvider Instance;

      GoogleChinaMapProvider()
      {
         RefererUrl = "http://ggmap01.bosim.top:8880/";
      }

      static GoogleChinaMapProvider()
      {
         Instance = new GoogleChinaMapProvider();
      }

      public override PureProjection Projection
      {
          get
          {
              return MercatorProjectionGCJ.Instance;
          }
      }

      public string Version = "m@218";

      #region GMapProvider Members

      readonly Guid id = new Guid("1213F763-64EE-4AB6-A14A-D84D6BCC3426");
      public override Guid Id
      {
         get
         {
            return id;
         }
      }

      readonly string name = "Google 地图";
      public override string Name
      {
         get
         {
            return name;
         }
      }

      public override PureImage GetTileImage(GPoint pos, int zoom)
      {
         string url = string.Format("http://ggmap01.bosim.top:8880/vt/lyrs=m&hl=zh-CN&gl=CN&src=app&x={0}&y={1}&z={2}", pos.X, pos.Y, zoom);
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
