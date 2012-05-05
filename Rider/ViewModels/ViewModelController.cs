using GalaSoft.MvvmLight;
using Rider.Location;

namespace Rider.ViewModels
{
    public class ViewModelController
    {
        private static MainViewModel mainViewModel = null;
        private static FluxViewModel fluxViewModel = null;
        private static MapViewModel mapViewModel = null;

        private static LocationService locationservice = null;

        public ViewModelController()
        { }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic", Justification = "This non-static member is needed for data binding purposes.")]

        public static MainViewModel MainViewModel
        {
            get
            {
                // Différer la création du modèle de vue autant que nécessaire
                if (mainViewModel == null)
                    mainViewModel = new MainViewModel();

                return mainViewModel;
            }
        }

        public static FluxViewModel FluxViewModel
        {
            get
            {
                // Différer la création du modèle de vue autant que nécessaire
                if (fluxViewModel == null)
                    fluxViewModel = new FluxViewModel();

                return fluxViewModel;
            }
        }

        public static MapViewModel MapViewModel
        {
            get
            {
                // Différer la création du modèle de vue autant que nécessaire
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
                    locationservice = new LocationService();

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
    }
}