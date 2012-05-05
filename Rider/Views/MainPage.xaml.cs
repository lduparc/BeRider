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
            // load data
            if (!ViewModelController.MainViewModel.IsDataLoaded) ViewModelController.MainViewModel.LoadData();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            int selection =(int)(sender as HyperlinkButton).Tag;
            this.panorama.DefaultItem = this.panorama.Items[selection];
        }

        private void GoToPageLink(object sender, RoutedEventArgs e)
        {
            string link = (sender as Control).Tag.ToString();
            NavigationService.Navigate(new Uri(link, UriKind.Relative));
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

    }

}