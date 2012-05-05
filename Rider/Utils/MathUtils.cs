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
using System.Device.Location;

namespace Rider.Utils
{
    public class MathUtils
    {
        private static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        
        private static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        public static int CourseFromPoint(GeoCoordinate point1, GeoCoordinate point2)
        {
            double lat1 = DegreeToRadian(point1.Latitude);
            double lng1 = DegreeToRadian(point1.Longitude);
            double lat2 = DegreeToRadian(point2.Latitude);
            double lng2 = DegreeToRadian(point2.Longitude);
            double dlng = lng2 - lng1;

            double x = Math.Sin(dlng) * Math.Cos(lat2);
            double y = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dlng);

            int bearing = (int)Math.Round(RadianToDegree(Math.Atan2(x, y)));
            return (bearing + 360) % 360;
        }

        public static double SlopeFromPoint(GeoCoordinate point1, GeoCoordinate point2)
        {
            double x = point2.Latitude - point1.Latitude;
            double y = point2.Longitude - point1.Longitude;

            return y == 0 ? 0 : (x / y);
        }

        public static double InterceptFromPoint(GeoCoordinate point1, GeoCoordinate point2)
        {
            double x = point2.Latitude - point1.Latitude;
            double y = point2.Longitude - point1.Longitude;

            return y == 0 ? 0 : (point1.Latitude - (x / y) * point1.Longitude);
        }

        public static double DistanceFromPoint(GeoCoordinate point, GeoCoordinate point1, GeoCoordinate point2)
        {
            double slope = SlopeFromPoint(point1, point2);
            double intercept = InterceptFromPoint(point1, point2);

            double x = slope * point.Longitude - point.Latitude + intercept;
            double y = Math.Sqrt(slope * slope + 1);

            return (y == 0) ? 0 : Math.Abs(x / y);
        }

        public static int AngleBetweenPoint(GeoCoordinate point1, GeoCoordinate point2, double bearing)
        {
            int alpha = CourseFromPoint(point2, point1);
            int gamma = (int)Math.Abs(bearing - alpha);
            return gamma > 180 ? 360 - gamma : gamma;
        }
    }
}
