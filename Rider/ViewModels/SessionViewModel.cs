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
using System.Collections.Generic;
using System.Linq;
using Rider.Models;
using Rider.Persistent;
using Microsoft.Phone.Controls.Maps;
using System.Device.Location;

namespace Rider.ViewModels
{
    public class SessionViewModel
    {
        public static readonly string TABLE_NAME = "sessions";
        public static readonly string ID_COLUMN_NAME = "id";
        public static readonly string TITLE_COLUMN_NAME = "title";
        public static readonly string DETAILS_COLUMN_NAME = "details";
        public static readonly string DISTANCE_COLUMN_NAME = "distance";
        public static readonly string DURATION_COLUMN_NAME = "duration";
        public static readonly string AVERAGE_SPEED_COLUMN_NAME = "average_speed";
        public static readonly string MAX_SPEED_COLUMN_NAME = "max_speed";
        public static readonly string KCAL_COLUMN_NAME = "kcal";
        public static readonly string SPORT_COLUMN_NAME = "sport";
        public static readonly string DATE_COLUMN_NAME = "date";

        private DateTime startTime;
        private DateTime endTime;
        private double distance;
        public List<double> AverageSpeeds { get; set; }
        private double averageSpeed;
        private LocationCollection coords;
        private string identifier;
        private int kcal;
        private string title = "Session #1";
        private string details = "some description";
        private int sport;
        private double maxSpeed;
        private string duration;
        private string dateHistory;

        public SessionViewModel()
        {
        }

        #region properties

        public string Identifer
        {
            get
            { return this.identifier; }
            set
            {
                this.identifier = value;
            }
        }

        public LocationCollection Coords
        {
            get
            {
                return this.coords;
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        public string Details
        {
            get
            {
                return this.details;
            }
            set
            {
                this.details = value;
            }
        }

        public int Sport
        {
            get
            {
                return this.sport;
            }
            set
            {
                this.sport = value;
            }
        }

        public DateTime StartTime
        {
            get
            {
                return this.startTime;
            }
            set
            {
                this.startTime = value;
            }
        }

        public DateTime EndTime
        {
            get
            {
                return this.endTime;
            }
            set
            {
                this.endTime = value;
            }
        }

        public string DistanceFormated
        {
            get
            {
                return string.Format(" {0:0.0} {1}", Models.Distance.MetersToUserDataUnit(Distance), UserData.Get<Speed.Unit>(UserData.UnitKey).ToString()).Replace(',', '.');
            }
        }

        public string AverageSpeedFormated
        {
            get
            {
                return string.Format("{0:0.0} {1}/h", Models.Speed.MetersToUserSpeedUnit(AverageSpeed), UserData.Get<Speed.Unit>(UserData.UnitKey).ToString()).Replace(',', '.');
            }
        }

        public string FormatedSpentTime
        {
            get
            {
                return string.Format("{0:00}H{1:00}min{2:00}s", (int)SpentTime.TotalHours, SpentTime.Minutes, SpentTime.Seconds);
            }
        }

        public TimeSpan SpentTime
        {
            get
            {
                return (this.endTime - this.startTime);
            }
        }

        public string DurationHistory
        {
            get
            {
                return this.duration;
            }
            set
            {
                this.duration = value;
            }
        }

        public string DateHistory
        {
            get
            {
                return this.dateHistory;
            }
            set
            {
                this.dateHistory = value;
            }
        }

        public double Distance
        {
            get
            { 
                return this.distance; 
            }
            set
            {
                this.distance = value;
            }
        }

        public double MaxSpeed
        {
            get
            {
                return this.maxSpeed;
            }
            set
            {
                this.maxSpeed = value;
            }
        }

        public double AverageSpeed
        {
            get
            {
                if (this.AverageSpeeds == null)
                    this.AverageSpeeds = new List<double>();
                return this.AverageSpeeds.Count > 0 ? this.AverageSpeeds.Average() : 0;
            }
        }

        public double AverageSpeedHistory
        {
            get
            {
                return this.averageSpeed;
            }
            set
            {
                this.averageSpeed = value;
            }

        }

        // TODO : calculate KCal
        public int KCal
        {
            get
            { return this.kcal; }
            set
            {
                this.kcal = value;
            }

        }

        #endregion

        #region utils

        public void AddNewSpeed(double speed)
        {
            if (this.AverageSpeeds == null)
                this.AverageSpeeds = new List<double>();
            this.AverageSpeeds.Add(speed);
        }

        public void AddNewCoord(double latitude, double longitude)
        {
            if (this.coords == null)
                this.coords = new LocationCollection();
            this.coords.Add(new GeoCoordinate(latitude, longitude));
        }

        public void AddNewDistance(double dist)
        {
            this.distance += dist;
        }

        #endregion



    }
}
