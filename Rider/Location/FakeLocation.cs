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
using System.Threading;
using System.Collections.Generic;
using System.Windows.Threading;
using System.ComponentModel;
using Rider.Persistent;

namespace Rider.Location
{

    abstract public class FakeLocation : IGeoPositionWatcher<GeoCoordinate>
    {
        abstract protected void StartGetCurrentPosition();

        public event EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>> PositionChanged;
        public event EventHandler<GeoPositionStatusChangedEventArgs> StatusChanged;

        protected BackgroundWorker asyncLocationWorker;

        private const string TAG = "FakeGeoLocation";
        private GeoPosition<GeoCoordinate> _current;
        private DispatcherTimer timer;

        public FakeLocation()
        {
            _current = new GeoPosition<GeoCoordinate>();
            Status = GeoPositionStatus.Initializing;
            RaiseStatusChanged();
        }

        private void RaiseStatusChanged()
        {
            GeoPositionStatusChangedEventArgs args = new GeoPositionStatusChangedEventArgs(this.Status);
            if (StatusChanged != null)
            {
                StatusChanged(this, args);
            }
        }

        private void RaisePositionChanged()
        {
            GeoPositionChangedEventArgs<GeoCoordinate> args = new GeoPositionChangedEventArgs<GeoCoordinate>(_current);
            if (PositionChanged != null)
            {
                PositionChanged(this, args);
            }
        }

        public void OnTimerCallback()
        {
            if (Status == GeoPositionStatus.Initializing)
            {
                Status = GeoPositionStatus.NoData;
                RaiseStatusChanged();
            }
            StartGetCurrentPosition();
        }

        protected void UpdateLocation(FakeGeoCoordinateObject loc)
        {
            GeoCoordinate location = new GeoCoordinate(loc.Latitude, loc.Longitude, 0, 0, 0, 0, loc.Course);
            if (!location.Equals(_current.Location))
            {
                _current = new GeoPosition<GeoCoordinate>(DateTimeOffset.Now, location);
                if (Status != GeoPositionStatus.Ready)
                {
                    Status = GeoPositionStatus.Ready;
                    RaiseStatusChanged();
                }

                RaisePositionChanged();
            }
        }

        public GeoPositionPermission Permission
        {
            get { return GeoPositionPermission.Granted; }
        }

        public GeoPosition<GeoCoordinate> Position
        {
            get { return _current; }
        }

        public void Start(bool suppressPermissionPrompt)
        {
            Start();
        }

        public void Start()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, Configuration.FAKE_LOCATION_DELAY / 1000);
            timer.Tick += delegate(object s, EventArgs args) {
                OnTimerCallback();
            };

            timer.Start();
        }

        public GeoPositionStatus Status
        {
            get;
            protected set;
        }

        public void Stop()
        {
            if (this.asyncLocationWorker != null && this.asyncLocationWorker.WorkerSupportsCancellation)
            {
                this.asyncLocationWorker.CancelAsync();
                this.asyncLocationWorker = null;
            }
            if (this.timer != null && this.timer.IsEnabled)
                this.timer.Stop();
            this.Status = GeoPositionStatus.Disabled;
            this.RaiseStatusChanged();
        }

        public bool TryStart(bool suppressPermissionPrompt, TimeSpan timeout)
        {
            Start();
            return true;
        }

    }

}
