using System;

namespace Rider.Maps
{
    public class OsmaRender : BaseTileSource
    {
        public OsmaRender()
        {
            UriFormat = "http://{0}.tah.openstreetmap.org/Tiles/tile/{1}/{2}/{3}.png";
        }

        private readonly static string[] TilePathPrefixes = 
            new[] { "a", "b", "c", "d", "e", "f" };

        public override Uri GetUri(int x, int y, int zoomLevel)
        {
            if (zoomLevel > 0)
            {             
                var url = string.Format(UriFormat, 
                    TilePathPrefixes[(y%3) + (3*(x%2))], zoomLevel, x, y);
                return new Uri(url);
            }
            return null;
        }
    }
}
