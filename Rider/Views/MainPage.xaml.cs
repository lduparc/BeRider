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
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using Rider.ViewModels;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;
using Rider.Resources;
using GalaSoft.MvvmLight.Messaging;
using Rider.Tracking;
using Rider.Models;
using Rider.Persistent;
using Rider.Utils;
using System.Threading;

namespace Rider.Views
{
    public partial class MainPage : PhoneApplicationPage
    {

        // Constructeur
        public MainPage()
        {
            InitializeComponent();
            InitializeApplicationBar();

            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            this.DataContext = ViewModelController.MainViewModel;

            bool isStarted = ViewModelController.TrackingService.IsRunning;
            sessionButton.Content = AppResource.ResourceManager.GetString(isStarted ? "sessionStopAppBarTitle" : "sessionStartAppBarTitle");
        }

        private void InitializeApplicationBar()
        {
            ApplicationBarIconButton mapButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            if (mapButton != null)
                mapButton.Text = AppResource.ResourceManager.GetString("MapAppBarTitle");
            ApplicationBarIconButton aboutButton = ApplicationBar.Buttons[1] as ApplicationBarIconButton;
            if (aboutButton != null)
                aboutButton.Text = AppResource.ResourceManager.GetString("AboutAppBarTitle");
            ApplicationBarIconButton settingsButton = ApplicationBar.Buttons[2] as ApplicationBarIconButton;
            if (settingsButton != null)
                settingsButton.Text = AppResource.ResourceManager.GetString("SettingsAppBarTitle");
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            OnSessionStatusChanged(ViewModelController.TrackingService.IsRunning);
           // OnUnitChanged(UserData.Get<Speed.Unit>(UserData.UnitKey));
            Messenger.Default.Register<bool>(this, TrackingService.SessionStatusChanged, status => OnSessionStatusChanged(status));
          //  Messenger.Default.Register<Speed.Unit>(this, UserData.UnitChanged, unit => OnUnitChanged(unit));

            // load data
            if (!ViewModelController.MainViewModel.IsDataLoaded) ViewModelController.MainViewModel.LoadDesignData();
        }

        private void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<bool>(this);
            Messenger.Default.Unregister<Speed.Unit>(this);
        }

        //private void OnUnitChanged(Speed.Unit unit)
        //{
        //    speedText.Text = string.Format("{0}/h", unit.ToString());
        //    distanceText.Text = unit.ToString();
        //}


        private void OnSessionStatusChanged(bool status)
        {
            if (UserData.Get<bool>(UserData.LocationToggleKey))
                sessionButton.Content = status ? AppResource.ResourceManager.GetString("sessionStopAppBarTitle") : AppResource.ResourceManager.GetString("sessionStartAppBarTitle");
            else sessionButton.Content = AppResource.ResourceManager.GetString("Settings");
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            int selection = (int)(sender as HyperlinkButton).Tag;
            this.panorama.DefaultItem = this.panorama.Items[selection];
        }

        private void carte_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/MapPage.xaml", UriKind.Relative));
        }

        private void about_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/About.xaml", UriKind.Relative));
        }

        private void parametre_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Settings.xaml", UriKind.Relative));
        }

        private void Session_Click(object sender, RoutedEventArgs e)
        {
            if (UserData.Get<bool>(UserData.LocationToggleKey))
            {
                bool isStarted = ViewModelController.TrackingService.IsRunning;
                if (isStarted)
                    ViewModelController.TrackingService.StopSession();
                else
                    ViewModelController.TrackingService.StartSession();
            }
            else
            {
                NavigationService.Navigate(new Uri("/Views/Settings.xaml", UriKind.Relative));
            }

        }

        private void panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (panorama.SelectedIndex)
            {
                case 3:
                    ViewModelController.MainViewModel.LoadSessionsSaved();
                    break;
                case 2:
                    // TODO : news
                    break;
                case 1:
                    // TODO : Spots
                    break;
                case 0:
                default:
                    break;
            }
        }

        private void historyShareMenuAction_Click(object sender, RoutedEventArgs e)
        {

        }

        private void historyDeleteMenuAction_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}