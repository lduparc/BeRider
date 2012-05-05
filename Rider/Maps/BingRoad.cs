namespace Rider.Maps
{
    public class BingRoad : BaseBingSource
    {
        public BingRoad()
        {
            UriFormat = "http://r{0}.ortho.tiles.virtualearth.net/tiles/r{1}.png?g=203";
        }
    }
}
