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
using System.Collections.Generic;

namespace Rider.ViewModels
{
    public class RssViewModel : BaseViewModel
    {
        private string _title;
        private string _description;
        private string _link;
        private string _guid;
        private DateTime _published;

        public string Title
        {
            get { return _title; }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        public string Description
        {
            get { return _description; }
            set 
            {
                if (value != _description)
                {
                    _description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        public string Link
        {
            get { return _link; }
            set 
            {
                if (value != _link)
                {
                    _link = value;
                    NotifyPropertyChanged("Link");
                }
            }
        }

        public string Guid
        {
            get { return _guid; }
            set
            {
                if (value != _guid)
                {
                    _guid = value;
                    NotifyPropertyChanged("Guid");
                }
            }
        }

        public DateTime Published
        {
            get { return _published; }
            set
            {
                if (value != _published)
                {
                    _published = value;
                    NotifyPropertyChanged("Published");
                }
            }
        }
    }
}
