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
using GalaSoft.MvvmLight.Command;
using System.Threading;
using System.Windows.Navigation;


namespace Rider.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public static readonly string SessionLoaded = "SessionLoaded";
        public static readonly string ShareSessionKey = "ShareSessionKey";

        private string _panoramaHomeTitle = "";
        private string _panoramaHistoryTitle = "";
        private string _panoramaSpotlightsTitle = "";
        private string _panoramaNewsTitle = "";
        private ICommand removeCommand = null;
        private ICommand shareCommand = null;
        private ObservableCollection<SessionViewModel> sessions;

        public MainViewModel()
        {
            this.removeCommand = new RelayCommand<SessionViewModel>(this.RemoveAction);
            this.shareCommand = new RelayCommand<SessionViewModel>(this.ShareAction);
            Messenger.Default.Register<Speed.Unit>(this, UserData.UnitChanged, unit => OnUnitChanged(unit));
            Messenger.Default.Register<bool>(this, SessionPageSelectionViewModel.sessionLoaded, status => OnSessionsLoaded(status));
        }

        public void UnRegisterCallback()
        {
            Messenger.Default.Unregister<bool>(this);
            Messenger.Default.Unregister<Speed.Unit>(this);
        }

        private void OnSessionsLoaded(bool status)
        {
            if (status)
                Sessions = ViewModelController.Sessions;
        }

        #region properties

        public ObservableCollection<SessionViewModel> Sessions
        {
            get
            {
                return this.sessions;
            }
            set
            {
                if (value != null)
                {
                    this.sessions = value;
                    NotifyPropertyChanged("Sessions");
                    Messenger.Default.Send<object>(null, MainViewModel.SessionLoaded);
                }
            }
        }

        private void OnUnitChanged(Speed.Unit unit)
        {
            NotifyPropertyChanged("UnitChanged");
        }
        
        public string UnitChanged
        {
            get
            {
                return UserData.Get<Speed.Unit>(UserData.UnitKey).ToString();
            }
        }

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

        public string PanoramaSpotlightsTitle
        {
            get
            {
                return _panoramaSpotlightsTitle;
            }
            set
            {
                if (value != _panoramaSpotlightsTitle)
                {
                    _panoramaSpotlightsTitle = value;
                    NotifyPropertyChanged("PanoramaSpotlightsTitle");
                }
            }
        }

        public string PanoramaNewsTitle
        {
            get
            {
                return _panoramaNewsTitle;
            }
            set
            {
                if (value != _panoramaNewsTitle)
                {
                    _panoramaNewsTitle = value;
                    NotifyPropertyChanged("PanoramaNewsTitle");
                }
            }
        }

        public bool IsDataLoaded { get; private set; }

        #endregion

        #region action

        public ICommand RemoveCommand
        {
            get
            {
                return this.removeCommand;
            }
        }

        public ICommand ShareCommand
        {
            get
            {
                return this.shareCommand;
            }
        }

        private void RemoveAction(SessionViewModel session)
        {
            if (session != null)
            {
                bool success = App.database.DeleteWithIdentifier(SessionViewModel.TABLE_NAME, session.Identifer);
                if (success)
                {
                    ViewModelController.Sessions.Remove(session);
                }
            }
        }

        private void ShareAction(SessionViewModel session)
        {
            if (session != null)
            {
                string title = "", link = "";
                    link = session.Identifer;
                    title = session.Title;
                Uri uri = new Uri("/Views/ShareSelectionPage.xaml?link=" + HttpUtility.HtmlEncode(link) + "&title=" + title, UriKind.Relative);
                Messenger.Default.Send<Uri>(uri, ShareSessionKey);
                //MessageBox.Show(string.Format("Session Identifier: {0}", session.Identifer));
            }
        }

        #endregion

        #region loaders

        public void LoadDesignData()
        {
            this.PanoramaHomeTitle = AppResource.ResourceManager.GetString("PanoramaHomeTitle");
            this.PanoramaHistoryTitle = AppResource.ResourceManager.GetString("PanoramaHistoryTitle");
            this.PanoramaSpotlightsTitle = AppResource.ResourceManager.GetString("PanoramaSpotlightsTitle");
            this.PanoramaNewsTitle = AppResource.ResourceManager.GetString("PanoramaNewsTItle");
            this.IsDataLoaded = true;
        }

        #endregion

    }
}