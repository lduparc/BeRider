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

namespace Rider.Persistent
{
    public static class Configuration
    {
        private static string[] ROADS = new string[] 
        { 
            "bordeaux_rocade",  // => 0
            "thiers"            // => 1
        };

        public static bool FAKE_LOCATION_ENABLED = true;    // false to use real gps listener
        public static string FAKE_JSON_ROAD = ROADS[1];     // change code here to set specific road
        public static int FAKE_LOCATION_DELAY = 700;        // in millisecond
        public static int FAKE_MIN_SPEED = 5;               // Meter per seconds
        public static int FAKE_MAX_SPEED = 15;              // Meter per seconds

    }
}
