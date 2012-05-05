using GalaSoft.MvvmLight;
using Rider.Location;
using Rider.Tracking;

namespace Rider.ViewModels
{
    public class ViewModelController
    {
        private static MainViewModel mainViewModel = null;
        private static FluxViewModel fluxViewModel = null;
        private static MapViewModel mapViewModel = null;

        private static LocationService locationservice = null;
        private static TrackingService trackingService = null;

        public ViewModelController() { }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic", Justification = "This non-static member is needed for data binding purposes.")]

        public static MainViewModel MainViewModel
        {
            get
            {
                // Diff�rer la cr�ation du mod�le de vue autant que n�cessaire
                if (mainViewModel == null)
                    mainViewModel = new MainViewModel();

                return mainViewModel;
            }
        }

        public static FluxViewModel FluxViewModel
        {
            get
            {
                // Diff�rer la cr�ation du mod�le de vue autant que n�cessaire
                if (fluxViewModel == null)
                    fluxViewModel = new FluxViewModel();

                return fluxViewModel;
            }
        }

        public static MapViewModel MapViewModel
        {
            get
            {
                // Diff�rer la cr�ation du mod�le de vue autant que n�cessaire
                if (mapViewModel == null)
                    mapViewModel = new MapViewModel();

                return mapViewModel;

            }
        }

        #region LocationService

        public static LocationService LocationService
        {
            get
            {
                if (locationservice == null)
                    locationservice = Location.LocationService.Instance;

                return locationservice;
            }
        }

        public static void StartLocationService()
        {
            LocationService.StartLocationWatcher();
        }

        public static void StopLocationService()
        {
            LocationService.StopLocationWatcher();
        }

        #endregion

        #region TrackingService

        public static TrackingService TrackingService
        {
            get
            {
                if (trackingService == null)
                    trackingService = Tracking.TrackingService.Instance;

                return trackingService;
            }
        }

        public static void StartTrackingService()
        {
            if (!TrackingService.IsRunning)
                TrackingService.StartSession();
        }

        public static void StopTrackingService()
        {
            if (TrackingService.IsRunning)
                TrackingService.StopSession();
        }

        #endregion

    }
}