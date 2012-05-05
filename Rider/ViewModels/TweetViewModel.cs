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
using System.Runtime.Serialization;

namespace Rider.ViewModels
{
    public class TweetViewModel : BaseViewModel
    {
        private string _author;
        private DateTime _published;
        private string _title;
        private string _avatar;

        public string Author
        {
            get { return _author; }
            set
            {
                if (value != _author)
                {
                    _author = value;
                    NotifyPropertyChanged("Author");
                }
            }
        }

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

        public string Avatar
        {
            get { return _avatar; }
            set
            {
                if (value != _avatar)
                {
                    _avatar = value;
                    NotifyPropertyChanged("Avatar");
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
