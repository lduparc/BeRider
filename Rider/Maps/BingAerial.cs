namespace Rider.Maps
{
    public class BingAerial : BaseBingSource
    {
        public BingAerial()
        {
            UriFormat = "http://h{0}.ortho.tiles.virtualearth.net/tiles/h{1}.jpeg?g=203";
        }
    }
}
