using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using Rider.Persistent;

namespace Rider.ViewModels
{
    public class SessionPageSelectionViewModel : BaseViewModel
    {
        public static readonly string sessionLoaded = "SessionLoaded";
        private ObservableCollection<SessionViewModel> sessions;
        private ICommand removeCommand = null;
        private ICommand shareCommand = null;

        public SessionPageSelectionViewModel()
        {
            this.removeCommand = new RelayCommand<SessionViewModel>(this.RemoveAction);
            this.shareCommand = new RelayCommand<SessionViewModel>(this.ShareAction);
            Messenger.Default.Register<bool>(this, SessionPageSelectionViewModel.sessionLoaded, status => OnSessionsLoaded(status));
        }

        public void UnRegisterCallback()
        {
            Messenger.Default.Unregister<bool>(this);
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
                string sport = "", duration = "", distance = "", kcal = "", average_speed = "";
                sport = session.SportFormated;
                duration = session.FormatedSpentTime;
                distance = session.DistanceFormated;
                kcal = session.KCalFormated;
                average_speed = session.AverageSpeedFormated;
                Uri uri = new Uri("/Views/ShareSelectionPage.xaml?sport=" + sport + "&duration=" + duration + "&distance=" + distance + "&kcal=" + kcal + "&average_speed=" + average_speed, UriKind.Relative);
                Messenger.Default.Send<Uri>(uri, MainViewModel.ShareSessionKey);
            }
        }

        #endregion

    }
}
