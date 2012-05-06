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
using Rider.Utils;
using Rider.Models;
using GalaSoft.MvvmLight.Messaging;
using System.Device.Location;
using Rider.ViewModels;
using System.Windows.Threading;
using System.Globalization;
using System.Collections.Generic;
using Rider.Persistent;
using System.Collections.ObjectModel;

namespace Rider.Tracking
{
    public class TrackingService : BaseViewModel
    {
        public static string SessionStatusChanged = "SessionStatusChanged";

        private bool isRunnig;
        private static TrackingService instance;
        private SessionViewModel currentSession;
        private GeoCoordinate lastLocation;
        private DispatcherTimer timer;

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
            // TODO : register and/or send currentSession
            DebugUtils.Log("trololo", "currentSession start at : " + currentSession.StartTime);
            DebugUtils.Log("trololo", "currentSession end at : " + currentSession.EndTime);
            DebugUtils.Log("trololo", "currentSession spent time : " +currentSession.SpentTime);

            DebugUtils.Log("trololo", "currentSession distance : " + currentSession.Distance);
            DebugUtils.Log("trololo", "currentSession vitesse max : " + Speed.MetersToUserSpeedUnit(currentSession.MaxSpeed));
            DebugUtils.Log("trololo", "currentSession vitesse moyenne : " + Speed.MetersToUserSpeedUnit(currentSession.AverageSpeed));
            DebugUtils.Log("trololo", "currentSession calories lost : " + currentSession.KCal);
            this.isRunnig = false;
            timer.Stop();
            SaveCurrentSession();
            Messenger.Default.Send<bool>(this.isRunnig, TrackingService.SessionStatusChanged);
        }

        private void SaveCurrentSession()
        {
            ObservableCollection<SessionViewModel> listSessions = UserData.Get<ObservableCollection<SessionViewModel>>(UserData.ListSessionKey);
            if (listSessions == null) listSessions = new ObservableCollection<SessionViewModel>();            
            listSessions.Add(currentSession);
            UserData.Add<ObservableCollection<SessionViewModel>>(UserData.ListSessionKey, listSessions);
            currentSession = null;
            NotifyPropertyChanged("TimeSpent");
            NotifyPropertyChanged("AverageSpeed");
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
                lastLocation = newLocation;
            }
        }

        private void UpdateGUI(object sender, EventArgs e)
        {
            NotifyPropertyChanged("TimeSpent");
            NotifyPropertyChanged("AverageSpeed");
            NotifyPropertyChanged("DistanceSession");
            NotifyPropertyChanged("KCal");
        }
    }
}
