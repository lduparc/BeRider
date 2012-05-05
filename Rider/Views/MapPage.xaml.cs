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

namespace Rider.Views
{
    public partial class MapPage : PhoneApplicationPage
    {
        private Pushpin currentLocationPin;
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
            ApplicationBarMenuItem photoButton = ApplicationBar.MenuItems[2] as ApplicationBarMenuItem;
            if (aboutButton != null)
                aboutButton.Text = AppResource.ResourceManager.GetString("AboutAppBarTitle");
            if (settingsButton != null)
                settingsButton.Text = AppResource.ResourceManager.GetString("SettingsAppBarTitle");
            if (photoButton != null)
                photoButton.Text = AppResource.ResourceManager.GetString("PhotosAppBarTitle");
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Register<Pushpin>(this, MapViewModel.PinLocationChanged, pin => OnPinLocationChanged(pin));
        }

        private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<Pushpin>(this);
            if (Map.Children.Contains(currentLocationPin))
                Map.Children.Remove(currentLocationPin);
            pinLocationLoaded = false;
            currentLocationPin = null;
        }

        #endregion

        #region AppBarAction

        private void previous_Click(object sender, EventArgs e)
        {
            ViewModelController.MapViewModel.ShowPreviousMap();
            if (Map.Children.Contains(currentLocationPin))
                Map.Children.Remove(currentLocationPin);
            pinLocationLoaded = false;
            currentLocationPin = null;
        }

        private void next_Click(object sender, EventArgs e)
        {
            ViewModelController.MapViewModel.ShowNextMap();
            if (Map.Children.Contains(currentLocationPin))
                Map.Children.Remove(currentLocationPin);
            pinLocationLoaded = false;
            currentLocationPin = null;
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

        #endregion

        private void OnPinLocationChanged(Pushpin pin)
        {
            if ((int)pin.Tag == -1)
            {
                currentLocationPin = pin;

                if (!pinLocationLoaded)
                {
                    if (!Map.Children.Contains(currentLocationPin))
                        Map.Children.Add(currentLocationPin);
                    pinLocationLoaded = true;
                }

                if (lockMode) Map.Center = currentLocationPin.Location;
            }

        }
    }
}