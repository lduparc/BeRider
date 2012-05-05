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
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Tasks;
using Rider.Resources;

namespace Rider.Views
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
        }

        #region click

        private void Contact_Click(object sender, RoutedEventArgs e)
        {
            EmailComposeTask task = new EmailComposeTask();
            task.Subject = string.Format("{0} {1}", AppResource.ResourceManager.GetString("ApplicationName"), AppResource.ResourceManager.GetString("FeedBack"));
            task.To = "loic.duparc@gmail.com";
            task.Show();
        }

        private void Rate_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask review = new MarketplaceReviewTask();
            review.Show();
        }

        #endregion

    }
}