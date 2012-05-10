using System;using System.Net;using System.Windows;using System.Windows.Controls;using System.Windows.Documents;using System.Windows.Ink;using System.Windows.Input;using System.Windows.Media;using System.Windows.Media.Animation;using System.Windows.Shapes;namespace Rider.Persistent{    public static class Configuration    {        #region locationService        private static string[] ROADS = new string[]         {             "bordeaux_rocade",  // => 0            "thiers"            // => 1        };        public static bool FAKE_LOCATION_ENABLED = true;    // false to use real gps listener        public static string FAKE_JSON_ROAD = ROADS[1];     // change code here to set specific road        public static int FAKE_LOCATION_DELAY = 700;        // in millisecond        public static int FAKE_MIN_SPEED = 5;               // Meter per seconds        public static int FAKE_MAX_SPEED = 15;              // Meter per seconds        #endregion        #region share        //Facebook
        public static readonly string FacebookAppId = "285061924919083";        //Twitter
        public static readonly string ConsumerKey = "3RJEfEdtswtT3PptWUPsqw";
        public static readonly string ConsumerKeySecret = "yHeM6C0ItZjWl1kZFbQ6rwqCqAQhGAtdmuj68MIs";

        public static readonly string RequestTokenUri = "https://api.twitter.com/oauth/request_token";        public static readonly string OAuthVersion = "1.0";
        public static readonly string CallbackUri = "http://www.google.com";
        public static readonly string AuthorizeUri = "https://api.twitter.com/oauth/authorize";
        public static readonly string AccessTokenUri = "https://api.twitter.com/oauth/access_token";        #endregion

        #region sport scale factor

        public static double[] SportsFactor = new double[] {
        8.5,            //>Cycling BMX or mountain
        8,              //>Cycling 12-13.9 MPH
        10,             //>Cycling 14-15.9 MPH
        12.0,           //>Cycling 16-19 MPH
        16.0,           //">Cycling 20 MPH
        6,              //>Hiking cross-country
        4.5,            //>Jog 4 MPH, level
        7,              //>Rollerblade skating
        3.5,            //>Rowing, stationary light
        7,              //>Rowing, stationary moderate
        8.5,            //>Rowing, stationary vigorous
        8,              //>Running 5 MPH 12 min/mile
        10,             //>Running 6 MPH 10 min/mile
        12.5,           //>Running 7.5 MPH 8 min/mile
        14,             //>Running 8.6 MPH 7 min/mile
        16,             //>Running 10 MPH 6 min/mile
        5,              //>Skateboarding
        8,              //>Skiing cross-country, moderate speed
        6,              //>Skiing downhill, moderate effort
        9.5,            //>Ski machine general
        4,              //>Walk 3.5 MPH, level, brisk pace
        4,              //>Walk 4 MPH, level, very brisk pace
        4.5,            //>Walk 4.5 MPH, level, very, very brisk
        6,              //>Walk / Jog 6MPH alternating
        3.5,            //>Walk pleasure, walking dog
        };
       
        #endregion

    }}