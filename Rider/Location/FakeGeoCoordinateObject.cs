using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rider.Location
{
    public class FakeGeoCoordinateObject
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Course { get; set; }

        public override string ToString()
        {
            return string.Format("lat:{0} - lng:{1} - course:{2}", Latitude, Longitude, Course);
        }
    
    }
}
