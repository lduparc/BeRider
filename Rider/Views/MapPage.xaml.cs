using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Rider.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Shell;
using Rider.Resources;
using System.Data.Services.Client;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.GamerServices;
using Rider.Persistent;

namespace Rider.Views
{
    public partial class MapPage : PhoneApplicationPage
    {
        private Pushpin currentLocationPin;
        private MapPolyline mapPoly;
        private MapPolyline sessionPolyline;
        private bool pinLocationLoaded;
        private bool lockMode;

        public MapPage()
        {
            InitializeComponent();
            InitializeApplicationBar();

            pinLocationLoaded = false;
            lockMode = true;
            DataContext = ViewModelController.MapViewModel;
        }

        #region Load/Unload

        private void InitializeApplicationBar()
        {
            ApplicationBarIconButton previousModeButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            if (previousModeButton != null)
                previousModeButton.Text = AppResource.ResourceManager.GetString("PreviousMapTileAppBarTitle");
            ApplicationBarIconButton lockerButton = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
            if (lockerButton != null)
                lockerButton.Text = AppResource.ResourceManager.GetString("LockerAppBarTitle");
            ApplicationBarIconButton sessionButton = ApplicationBar.Buttons[2] as ApplicationBarIconButton;
            if (sessionButton != null)
            {
                bool isStarted = ViewModelController.TrackingService.IsRunning;
                sessionButton.Text = AppResource.ResourceManager.GetString(isStarted ? "sessionStopAppBarTitle" : "sessionStartAppBarTitle");
            }
            ApplicationBarIconButton nextModeButton = ApplicationBar.Buttons[3] as ApplicationBarIconButton;
            if (nextModeButton != null)
                nextModeButton.Text = AppResource.ResourceManager.GetString("NextMapTileAppBarTitle");

            ApplicationBarMenuItem aboutButton = ApplicationBar.MenuItems[0] as ApplicationBarMenuItem;
            ApplicationBarMenuItem settingsButton = ApplicationBar.MenuItems[1] as ApplicationBarMenuItem;
            ApplicationBarMenuItem loadSessionButton = ApplicationBar.MenuItems[2] as ApplicationBarMenuItem;
            if (aboutButton != null)
                aboutButton.Text = AppResource.ResourceManager.GetString("AboutAppBarTitle");
            if (settingsButton != null)
                settingsButton.Text = AppResource.ResourceManager.GetString("SettingsAppBarTitle");
            if (loadSessionButton != null)
                loadSessionButton.Text = AppResource.ResourceManager.GetString("SessionLoadAppBarTitle");
            ApplicationBarMenuItem unloadSessionButton = ApplicationBar.MenuItems[3] as ApplicationBarMenuItem;
            if (unloadSessionButton != null)
            {
                unloadSessionButton.IsEnabled = ViewModelController.MapViewModel.SessionPolylineLoaded;
                unloadSessionButton.Text = AppResource.ResourceManager.GetString("SessionUnloadAppBarTitle");
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Register<Pushpin>(this, MapViewModel.PinLocationChanged, pin => OnPinLocationChanged(pin));

            if (!UserData.Get<bool>(UserData.LocationToggleKey))
            {
                string title = AppResource.ResourceManager.GetString("LocationServiceTitle");
                string content = AppResource.ResourceManager.GetString("LocationServiceContent");
                string accept = AppResource.ResourceManager.GetString("LocationServiceAccept");
                string cancel = AppResource.ResourceManager.GetString("LocationServiceRefuse");
                Guide.BeginShowMessageBox(title, content, new string[] { accept, cancel }, 0, MessageBoxIcon.Alert, MessageBoxAcceptLocationService, null);
            }
        }

        private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<Pushpin>(this);
            CleanOverlays();
        }

        private void MessageBoxAcceptLocationService(IAsyncResult ar) 
        { 
            int? indexButton = Guide.EndShowMessageBox(ar); 
            if (indexButton.HasValue && indexButton.Value == 0) 
            { 
                Dispatcher.BeginInvoke(() => NavigationService.Navigate(new Uri("/Views/Settings.xaml", UriKind.Relative))); 
            }
        }

        #endregion

        private void CleanOverlays()
        {
            Map.Children.Clear();
            pinLocationLoaded = false;
            currentLocationPin = null;
            mapPoly = null;
        }

        #region AppBarAction

        private void previous_Click(object sender, EventArgs e)
        {
            CleanOverlays();
            ViewModelController.MapViewModel.ShowPreviousMap();
        }

        private void next_Click(object sender, EventArgs e)
        {
            CleanOverlays();
            ViewModelController.MapViewModel.ShowNextMap();
        }

        private void locker_Click(object sender, EventArgs e)
        {
            //TODO change icon
            lockMode = !lockMode;
        }

        private void session_Click(object sender, EventArgs e)
        {
            bool isStarted = ViewModelController.TrackingService.IsRunning;
            ApplicationBarIconButton sessionButton = ApplicationBar.Buttons[2] as ApplicationBarIconButton;

            if (isStarted)
            {
                ViewModelController.TrackingService.StopSession();
                sessionButton.Text = AppResource.ResourceManager.GetString("sessionStartAppBarTitle");
            }
            else
            {
                ViewModelController.TrackingService.StartSession();
                sessionButton.Text = AppResource.ResourceManager.GetString("sessionStartAppBarTitle");
            }
        }

        private void ApplicationBarMenuItemSettings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Settings.xaml", UriKind.Relative));
        }

        private void ApplicationBarMenuItemCapture_Click(object sender, EventArgs e)
        {
        }

        private void ApplicationBarMenuItemAbout_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/About.xaml", UriKind.Relative));
        }

        private void ApplicationBarMenuItemLoadSession_Click(object sender, EventArgs e)
        {
            if (ViewModelController.MainViewModel.Sessions.Count > 0)
            {
                if (sessionPolyline == null)
                {
                    sessionPolyline = new MapPolyline();
                    sessionPolyline.Stroke = new SolidColorBrush(Colors.Red);
                    sessionPolyline.StrokeThickness = 10;
                }

                LocationCollection collec = ViewModelController.TrackingService.currentSession.Coords;

                sessionPolyline.Locations = collec;
                if (!Map.Children.Contains(sessionPolyline))
                    Map.Children.Add(sessionPolyline);
                ViewModelController.MapViewModel.SessionPolylineLoaded = true;
                ApplicationBarMenuItem unloadSessionButton = ApplicationBar.MenuItems[3] as ApplicationBarMenuItem;
                if (unloadSessionButton != null)
                    unloadSessionButton.IsEnabled = true;
            }
        }

        private void ApplicationBarMenuItemUnloadSession_Click(object sender, EventArgs e)
        {
            if (Map.Children.Contains(sessionPolyline))
                Map.Children.Remove(sessionPolyline);
            ViewModelController.MapViewModel.SessionPolylineLoaded = false;
            ApplicationBarMenuItem unloadSessionButton = ApplicationBar.MenuItems[3] as ApplicationBarMenuItem;
            if (unloadSessionButton != null)
                unloadSessionButton.IsEnabled = false;
        }


        #endregion

        private void OnPinLocationChanged(Pushpin pin)
        {
            if ((int)pin.Tag == -1)
            {
                if (currentLocationPin == null)
                    currentLocationPin = pin;
                if (mapPoly == null)
                    mapPoly = (this.DataContext as MapViewModel).PolylineMapping;
                if (!pinLocationLoaded)
                {
                    if (!Map.Children.Contains(mapPoly))
                        Map.Children.Add(mapPoly);
                    if (!Map.Children.Contains(currentLocationPin))
                        Map.Children.Add(currentLocationPin);
                    pinLocationLoaded = true;
                }

                if (lockMode) Map.Center = currentLocationPin.Location;
            }

        }

    }
}