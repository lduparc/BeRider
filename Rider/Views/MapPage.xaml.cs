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

namespace Rider.Views
{
    public partial class MapPage : PhoneApplicationPage
    {
        private Pushpin currentLocationPin;
        private bool pinLocationLoaded;

        public MapPage()
        {
            InitializeComponent();
            InitializeApplicationBar();
            pinLocationLoaded = false;
        }

        #region Load/Unload

        private void InitializeApplicationBar()
        {
            ApplicationBarIconButton aboutButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            if (aboutButton != null)
                aboutButton.Text = AppResource.ResourceManager.GetString("AboutAppBarTitle");
            ApplicationBarIconButton settingsButton = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
            if (settingsButton != null)
                settingsButton.Text = AppResource.ResourceManager.GetString("SettingsAppBarTitle");
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModelController.StartLocationService();
            Messenger.Default.Register<Pushpin>(this, MapViewModel.PinLocationChanged, pin => OnPinLocationChanged(pin));
            //Map.ZoomLevel = 15;
        }

        private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModelController.StopLocationService();
        }

        #endregion

        #region AppBarAction

        private void about_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/About.xaml", UriKind.Relative));
        }

        private void parametre_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Settings.xaml", UriKind.Relative));
        }

        #endregion

        private void OnPinLocationChanged(Pushpin pin)
        {
            if ((int)pin.Tag == -1)
            {
                currentLocationPin = pin;

                if (!pinLocationLoaded)
                {
                    Map.Children.Add(currentLocationPin);
                    pinLocationLoaded = true;
                }
            }
        
        }
    }
}