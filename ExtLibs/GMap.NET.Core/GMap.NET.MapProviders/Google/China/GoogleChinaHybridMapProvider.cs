
namespace GMap.NET.MapProviders
{
    using GMap.NET.Projections;
    using System;

   /// <summary>
   /// GoogleChinaHybridMap provider - 国内镜像 ggmap01.bosim.top:8880
   /// 卫星图(lyrs=s) + 标注层(lyrs=h) 叠加
   /// </summary>
   public class GoogleChinaHybridMapProvider : GoogleMapProviderBase
   {
      public static readonly GoogleChinaHybridMapProvider Instance;

      GoogleChinaHybridMapProvider()
      {
         RefererUrl = "http://ggmap01.bosim.top:8880/";
      }

      static GoogleChinaHybridMapProvider()
      {
         Instance = new GoogleChinaHybridMapProvider();
      }

      public override PureProjection Projection
      {
          get
          {
              return MercatorProjectionGCJ.Instance;
          }
      }

      public string Version = "h@264000000";

      #region GMapProvider Members

      readonly Guid id = new Guid("B8A2A78D-1C49-45D0-8F03-9B95C83116B7");
      public override Guid Id
      {
         get
         {
            return id;
         }
      }

      readonly string name = "Google 混合地图";
      public override string Name
      {
         get
         {
            return name;
         }
      }

      GMapProvider[] overlays;
      public override GMapProvider[] Overlays
      {
         get
         {
            if(overlays == null)
            {
               overlays = new GMapProvider[] { GoogleChinaSatelliteMapProvider.Instance, this };
            }
            return overlays;
         }
      }

      // 本层只返回透明标注层（道路/地名），叠在卫星图上
      public override PureImage GetTileImage(GPoint pos, int zoom)
      {
         string url = string.Format("http://ggmap01.bosim.top:8880/vt/lyrs=h&hl=zh-CN&gl=CN&src=app&x={0}&y={1}&z={2}", pos.X, pos.Y, zoom);
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
