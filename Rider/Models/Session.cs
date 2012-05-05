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

namespace Rider.Models
{
    public class Session
    {
        private DateTime startTime;
        private DateTime endTime;
        private double distance;
        private int maxSpeed;
        private List<double> averageSpeeds;
        private List<Coord> coords;
        private int kcal;

        public Session()
        {
            this.averageSpeeds = new List<double>();
            this.coords = new List<Coord>();
            startTime = DateTime.Now;
        }

        #region properties

        public DateTime StartTime
        {
            get
            { return this.startTime; }
        }

        public DateTime EndTime
        {
            get
            { return this.endTime; }
            set
            {
                this.endTime = value == null ? DateTime.Now : value; 
            }
        }

        public TimeSpan SpentTime
        {
            get
            { return (this.endTime - this.startTime); }
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
                return this.averageSpeeds.Count > 0 ? this.averageSpeeds.Max() : 0; 
            }
        }

        public double AverageSpeed
        {
            get 
            {
                return this.averageSpeeds.Count > 0 ? this.averageSpeeds.Average() : 0;
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
            this.averageSpeeds.Add(speed);
        }

        public void AddNewCoord(double latitude, double longitude)
        {
            this.coords.Add(new Coord(latitude, longitude));
            // TODO : calculate distance with last coord and newCoord;
        }

        public void AddNewDistance(double dist)
        {
            this.distance += dist;
        }

        #endregion



    }
}
