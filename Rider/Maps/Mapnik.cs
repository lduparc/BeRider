using System;

namespace Rider.Maps
{
    public class Mapnik : BaseTileSource
    {
        public Mapnik()
        {
            UriFormat = "http://{0}.tile.openstreetmap.org/{1}/{2}/{3}.png";
        }

        private readonly static string[] TilePathPrefixes = new[] { "a", "b", "c" };

        public override Uri GetUri(int x, int y, int zoomLevel)
        {
            if (zoomLevel > 0)
            {
                var url = string.Format(UriFormat, TilePathPrefixes[y%3], zoomLevel, x, y);
                return new Uri(url);
            }
            return null;
        }
    }
}


