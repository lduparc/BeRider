using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Rider.ViewModels
{
    public class ItemViewModel : BaseViewModel
    {
        private string _title = "";
        private string _details = "";
        private int _identifier;
        private string _smartInfo = "";
        private string _url = "";

        #region properties

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        public string SmartInfo
        {
            get
            {
                return _smartInfo;
            }
            set
            {
                if (value != _smartInfo)
                {
                    _smartInfo = value;
                    NotifyPropertyChanged("SmartInfo");
                }
            }
        }

        public int Identifier
        {
            get
            {
                return _identifier;
            }
            set
            {
                if (value != _identifier)
                {
                    _identifier = value;
                    NotifyPropertyChanged("Identifier");
                }
            }
        }

        public string Details
        {
            get
            {
                return _details;
            }
            set
            {
                if (value != _details)
                {
                    _details = value;
                    NotifyPropertyChanged("Details");
                }
            }
        }

        public string Link
        {
            get
            {
                return _url;
            }
            set
            {
                if (value != _url)
                {
                    _url = value;
                    NotifyPropertyChanged("Link");
                }
            }
        }
     
        #endregion

    }
}