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

namespace Rider.Models
{
    public class Coord
    {
        private double latitude;
        private double longitude;

        public Coord(double lat, double lng)
        {
            latitude = lat;
            longitude = lng;
        }

        public double Latitude
        {
            get { return latitude; }
        }

        public double Longitude
        {
            get { return longitude; }
        }
    
    }
}
