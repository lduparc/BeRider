﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Rider.Utils;
using System.Linq;
using Rider.Models;
using GalaSoft.MvvmLight.Messaging;
using System.Device.Location;
using Rider.ViewModels;
using System.Windows.Threading;
using System.Globalization;
using System.Collections.Generic;
using Rider.Persistent;
using System.Collections.ObjectModel;
using Rider.Location;
using Microsoft.Phone.Controls.Maps;

namespace Rider.Tracking
{
    public class TrackingService : BaseViewModel
    {
        public static string SessionStatusChanged = "SessionStatusChanged";

        private bool isRunnig;
        private static TrackingService instance;
        public SessionViewModel currentSession;
        private GeoCoordinate lastLocation;
        private DispatcherTimer timer;
        private double currentSpeed;

        #region constructors

        private TrackingService()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(UpdateGUI);
        }

        public static TrackingService Instance
        {
            get
            {
                if (instance == null)
                    instance = new TrackingService();
                return instance;
            }
        }

        #endregion

        #region properties

        public bool IsRunning
        {
            get { return this.isRunnig; }
        }

        public string TimeSpent
        {
            get
            {
                TimeSpan val = currentSession == null ? (DateTime.Now - DateTime.Now) : (DateTime.Now - currentSession.StartTime);
                return string.Format("{0:00}:{1:00}:{2:00}", (int)val.TotalHours, val.Minutes, val.Seconds);
            }

        }

        public string CurrentSpeed
        {
            get
            {
                return this.currentSpeed.ToString("0.0").Replace(',', '.');
            }
        }

        public string AverageSpeed
        {
            get
            {

                double moy = currentSession == null ? 0 : Speed.MetersToUserSpeedUnit(currentSession.AverageSpeed);
                return moy.ToString("0.0").Replace(',', '.');
            }
        }

        public string DistanceSession
        {
            get
            {
                double dist = currentSession == null ? 0 : currentSession.Distance;
                return Distance.MetersToUserDataUnit(dist).ToString("00.0").Replace(',', '.');
            }
        }

        public int KCal
        {
            get
            {
                return currentSession == null ? 0 : currentSession.KCal;
            }
        }

        #endregion

        #region manager

        public void StartSession()
        {
            Messenger.Default.Register<GeoCoordinate>(this, Location.LocationService.locationChanged, newLocation => OnLocationChanged(newLocation));
            currentSession = new SessionViewModel();
            currentSession.StartTime = DateTime.Now;
            timer.Start();
            this.isRunnig = true;
            DebugUtils.Log("trololo", "currentSession started");
            Messenger.Default.Send<bool>(this.isRunnig, TrackingService.SessionStatusChanged);
        }

        public void StopSession()
        {
            Messenger.Default.Unregister<GeoCoordinate>(this);
            currentSession.EndTime = DateTime.Now;
            this.isRunnig = false;
            timer.Stop();
            SaveCurrentSession();
            Messenger.Default.Send<bool>(this.isRunnig, TrackingService.SessionStatusChanged);
        }

        private void SaveCurrentSession()
        {
            int lastidx = UserData.Get<int>(UserData.SessionIndexKey);
            double maxSpeed = (currentSession.AverageSpeeds != null && currentSession.AverageSpeeds.Count > 0) ? currentSession.AverageSpeeds.Max() : 0;
            double averageSpeed = (currentSession.AverageSpeeds != null && currentSession.AverageSpeeds.Count > 0) ? currentSession.AverageSpeeds.Average() : 0;
            Dictionary<string, object> dictSession = new Dictionary<string, object>();
            dictSession.Add(SessionViewModel.ID_COLUMN_NAME, lastidx == -1 ? 0 : lastidx);
            dictSession.Add(SessionViewModel.TITLE_COLUMN_NAME, currentSession.Title);
            dictSession.Add(SessionViewModel.DETAILS_COLUMN_NAME, currentSession.Details);
            dictSession.Add(SessionViewModel.DISTANCE_COLUMN_NAME, currentSession.Distance);
            dictSession.Add(SessionViewModel.DURATION_COLUMN_NAME, currentSession.FormatedSpentTime);
            dictSession.Add(SessionViewModel.AVERAGE_SPEED_COLUMN_NAME, averageSpeed);
            dictSession.Add(SessionViewModel.MAX_SPEED_COLUMN_NAME, maxSpeed);
            dictSession.Add(SessionViewModel.KCAL_COLUMN_NAME, currentSession.KCal);
            dictSession.Add(SessionViewModel.SPORT_COLUMN_NAME, currentSession.Sport);
            bool success = App.database.InsertWithContent(SessionViewModel.TABLE_NAME, dictSession);

            Dictionary<string, object> dictLocations = new Dictionary<string, object>();
            foreach (GeoCoordinate loc in currentSession.Coords)
            {
                dictLocations.Clear();
                if (loc != null)
                {
                    dictLocations.Add(LocationService.SESSION_ID_COLUMN_NAME, lastidx == -1 ? 0 : lastidx);
                    dictLocations.Add(LocationService.LAT_COLUMN_NAME, loc.Latitude);
                    dictLocations.Add(LocationService.LNG_COLUMN_NAME, loc.Longitude);
                    success = App.database.InsertWithContent(LocationService.TABLE_NAME, dictLocations);
                }
            }

            if (success)
                UserData.Add<int>(UserData.SessionIndexKey, lastidx + 1);
            
            currentSession = null;
            currentSpeed = 0;
            NotifyPropertyChanged("TimeSpent");
            NotifyPropertyChanged("CurrentSpeed");
            NotifyPropertyChanged("DistanceSession");
            NotifyPropertyChanged("KCal");
        }
        #endregion

        private void OnLocationChanged(GeoCoordinate newLocation)
        {
            if (currentSession != null && newLocation != null)
            {
                currentSession.AddNewCoord(newLocation.Latitude, newLocation.Longitude);
                currentSession.AddNewSpeed(newLocation.Speed);
                if (lastLocation != null)
                    currentSession.AddNewDistance(newLocation.GetDistanceTo(lastLocation));
                this.currentSpeed = newLocation.Speed;
                lastLocation = newLocation;
            }
        }

        private void UpdateGUI(object sender, EventArgs e)
        {
            NotifyPropertyChanged("TimeSpent");
            NotifyPropertyChanged("CurrentSpeed");
            NotifyPropertyChanged("CurrentSpeed");
            NotifyPropertyChanged("DistanceSession");
            NotifyPropertyChanged("KCal");
        }
    }
}
