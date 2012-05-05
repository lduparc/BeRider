using System;

namespace Rider.Maps
{
    public class Google : BaseTileSource
    {
        public Google()
        {
            MapType = GoogleType.PhysicalHybrid;
            UriFormat = @"http://mt{0}.google.com/vt/lyrs={1}&z={2}&x={3}&y={4}";
        }

        public GoogleType MapType { get; set; }

        public override Uri GetUri(int x, int y, int zoomLevel)
        {
            return new Uri( 
                string.Format(UriFormat, (x % 2) + (2 * (y % 2)),
                (char)MapType, zoomLevel, x, y) );
        }
    }
}

