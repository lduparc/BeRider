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

namespace Rider.Models
{
    public class TwitterAccess
    {
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
        public string UserId { get; set; }
        public string ScreenName { get; set; }
    }
}
