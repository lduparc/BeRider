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
using Microsoft.Phone.Tasks;

namespace Rider.Views
{
    public partial class FluxPage : PhoneApplicationPage
    {
        public FluxPage()
        {
            InitializeComponent();
            DataContext = ViewModelController.FluxViewModel;
        }

        private void NewsListBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModelController.FluxViewModel.LoadNews();
            ViewModelController.FluxViewModel.LoadTweets();
        }

        private void PhoneApplicationPage_Unloaded(object sender, RoutedEventArgs e)
        {

        }

    }
}