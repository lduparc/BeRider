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

namespace Rider.Tracking
{
    public class TrackingService : BaseViewModel
    {
        private bool isRunnig;
        private static TrackingService instance;
        private Session currentSession;
        private GeoCoordinate lastLocation;
        private DispatcherTimer timer;
        private int seconds;
        private int minutes;
        private int hours;

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

        public string HoursTimeSpent
        {
            get
            {
                int val = currentSession == null ? 00 : (DateTime.Now - currentSession.StartTime).Hours;
                return string.Format("{0}{1}", val < 10 ? "0" : "", val);
            }

        }

        public string MinutesTimeSpent 
        {
            get 
            {
                int val = currentSession == null ? 00 : (DateTime.Now - currentSession.StartTime).Minutes;
                return string.Format("{0}{1}", val < 10 ? "0" : "", val);
            }

        }

        public string SecondsTimeSpent
        {
            get
            {
                int val = currentSession == null ? 00 : (DateTime.Now - currentSession.StartTime).Seconds;
                return string.Format("{0}{1}", val < 10 ? "0" : "", val);
            }

        }

        #endregion

        #region manager

        public void StartSession()
        {
            Messenger.Default.Register<GeoCoordinate>(this, Location.LocationService.locationChanged, newLocation => OnLocationChanged(newLocation));
            currentSession = new Session();
            timer.Start();
            this.isRunnig = true;
            DebugUtils.Log("trololo", "currentSession started");
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
            DebugUtils.Log("trololo", "currentSession vitesse max : " + currentSession.MaxSpeed);
            DebugUtils.Log("trololo", "currentSession vitesse moyenne : " + currentSession.AverageSpeed);
            DebugUtils.Log("trololo", "currentSession calories lost : " + currentSession.KCal);
            this.isRunnig = false;
            currentSession = null;
            timer.Stop();
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
            NotifyPropertyChanged("SecondsTimeSpent");
            NotifyPropertyChanged("MinutesTimeSpent");
            NotifyPropertyChanged("HoursTimeSpent");
        }
    }
}
