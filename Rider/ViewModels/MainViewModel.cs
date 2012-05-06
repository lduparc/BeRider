using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Rider;
using Rider.Persistent;
using System.IO;
using System.Xml;
using System.Net;
using Rider.Models;
using Rider.Resources;
using GalaSoft.MvvmLight.Messaging;
using Rider.Tracking;
using Rider.Utils;


namespace Rider.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<SessionViewModel> Sessions { get; private set; }
        private string _panoramaHomeTitle = "";
        private string _panoramaHistoryTitle = "";

        public MainViewModel()
        {
            this.Sessions = new ObservableCollection<SessionViewModel>();
        }

        #region properties

        public string PanoramaHomeTitle
        {
            get
            {
                return _panoramaHomeTitle;
            }
            set
            {
                if (value != _panoramaHomeTitle)
                {
                    _panoramaHomeTitle = value;
                    NotifyPropertyChanged("PanoramaHomeTitle");
                }
            }
        }

        public string PanoramaHistoryTitle
        {
            get
            {
                return _panoramaHistoryTitle;
            }
            set
            {
                if (value != _panoramaHistoryTitle)
                {
                    _panoramaHistoryTitle = value;
                    NotifyPropertyChanged("PanoramaHistoryTitle");
                }
            }
        }

        public bool IsDataLoaded { get; private set; }

        #endregion

        public void LoadDesignData()
        {
            this.PanoramaHomeTitle = AppResource.ResourceManager.GetString("PanoramaHomeTitle");
            this.PanoramaHistoryTitle = AppResource.ResourceManager.GetString("PanoramaHistoryTitle");
            this.IsDataLoaded = true;
        }

        public void LoadSessionsSaved()
        {
            Sessions.Clear();
            ObservableCollection<SessionViewModel> sessionList = UserData.Get<ObservableCollection<SessionViewModel>>(UserData.ListSessionKey);
            if (sessionList == null || sessionList.Count == 0)
            {
                // TODO : afficher text pas de sessions sauvegardees
            }
            else
            {
                foreach (SessionViewModel svm in sessionList)
                {
                    Sessions.Add(svm);
                }
                // TODO : afficher la liste
            }
        
        }

    }
}