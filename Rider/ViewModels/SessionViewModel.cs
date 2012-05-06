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
        private DateTime startTime;
        private DateTime endTime;
        private double distance;
        private List<double> AverageSpeeds {get; set;}
        private LocationCollection coords;
        private int kcal;

        public SessionViewModel()
        {
        }

        #region properties

        public LocationCollection Coords
        {
            get
            {
                return this.coords;
            }
        }

        public string Sport
        {
            get
            {
                return "Roller";
            }
        }

        public string SmartInfo
        {
            get
            {
                return "";
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

        public double Distance
        {
            get
            { return this.distance; }
            set
            {
                if (!double.IsNaN(value) && value > 0)
                {
                    this.distance += value;
                }
            }

        }

        public double MaxSpeed
        {
            get
            {
                return (this.AverageSpeeds != null && this.AverageSpeeds.Count > 0) ? this.AverageSpeeds.Max() : 0; 
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

        // TODO : calculate KCal
        public int KCal
        {
            get
            { return this.kcal; }
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
