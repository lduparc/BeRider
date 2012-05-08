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
using System.Collections.ObjectModel;
using Rider.Persistent;
using Rider.Utils;
using Rider.Resources;

namespace Rider.Views
{
    public partial class SessionsPageSelection : PhoneApplicationPage
    {
        private bool sessionsListLoaded;

        public SessionsPageSelection()
        {
            InitializeComponent();
            this.DataContext = ViewModelController.SessionPageSelectionViewModel;
            sessionsListLoaded = false;
            sessionsList.SelectedIndex = -1;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Register<object>(this, MainViewModel.SessionLoaded, obj => OnSessionLoaded());
            shellProgress.Text = AppResource.ResourceManager.GetString("LoadingSessions");
            shellProgress.IsVisible = true;
            ViewModelController.LoadSessionsSaved();
        }

        private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
        {
            (this.DataContext as SessionPageSelectionViewModel).UnRegisterCallback();
            Messenger.Default.Unregister<object>(this);
        }

        private void OnSessionLoaded()
        {
            shellProgress.IsVisible = false;
        }

        private void sessionsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sessionsListLoaded && sessionsList.SelectedIndex != -1)
            {
                UserData.Add(UserData.SessionSelectedIndexKey, sessionsList.SelectedIndex);
                NavigationService.GoBack();
            }
        }

        private void sessionsList_Loaded(object sender, RoutedEventArgs e)
        {
            sessionsListLoaded = true;
        }

    }
}