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
using System.IO;
using System.Xml;
using Rider.Persistent;
using Rider.Utils;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using GalaSoft.MvvmLight.Command;

namespace Rider.ViewModels
{
    public class FluxViewModel : BaseViewModel
    {
        public ObservableCollection<RssViewModel> WebsiteFeed { get; private set; }
        public ObservableCollection<RssViewModel> FacebookFeed { get; private set; }
        public ObservableCollection<TweetViewModel> TwitterFeed { get; private set; }

        private ICommand shareFeedCommand = null;
        private ICommand showFeedCommand = null;
        private ICommand shareTweetCommand = null;

        public FluxViewModel()
        {
            WebsiteFeed = new ObservableCollection<RssViewModel>();
            FacebookFeed = new ObservableCollection<RssViewModel>();
            TwitterFeed = new ObservableCollection<TweetViewModel>();

            this.shareFeedCommand = new RelayCommand<RssViewModel>(this.ShareFeedAction);
            this.showFeedCommand = new RelayCommand<RssViewModel>(this.ShowFeedAction);
            this.shareTweetCommand = new RelayCommand<TweetViewModel>(this.ShareTweetAction);
        }

        #region command

        public ICommand ShareFeedCommand
        {
            get
            {
                return this.shareFeedCommand;
            }
        }

        public ICommand ShowFeedCommand
        {
            get
            {
                return this.showFeedCommand;
            }
        }

        public ICommand ShareTweetCommand
        {
            get
            {
                return this.shareTweetCommand;
            }
        }

        private void ShareFeedAction(RssViewModel rss)
        {
            if (rss != null)
            {
                MessageBox.Show(string.Format("share rss: {0}", rss.Title));
            }
        }

        private void ShowFeedAction(RssViewModel rss)
        {
            if (rss != null)
            {
                MessageBox.Show(string.Format("rss: {0}", rss.Title));
            }
        }

        private void ShareTweetAction(TweetViewModel tweet)
        {
            if (tweet != null)
            {
                MessageBox.Show(string.Format("share tweet: {0}", tweet.Title));
            }
        }


        #endregion

        #region NewsUtils

        public void LoadNews()
        {
            List<RssViewModel> flux = UserData.Get<List<RssViewModel>>("news_rssfeed");
            // TEST
            if (flux == null) {
                flux = new List<RssViewModel>();
                flux.Add(new RssViewModel() 
                {
                    Title = "TEST TITLE",
                    Description = "some content",
                    Link = "http://cespage.com/silverlight/tutorials.xml"
                });
            }
             
            if (flux != null && flux.Count > 0)
            {
                WebClient clientFeed = new WebClient();
                clientFeed.DownloadStringCompleted += Weblient_DownloadStringCompleted;
                foreach (RssViewModel rss in flux)
                {
                    // todo : http web request sur chaque flux                
                    clientFeed.DownloadStringAsync(new Uri(rss.Link), rss.Title);
                }

                // todo : sort syndication feed by publish Date
            }
        }

        private void Weblient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            XElement _xml;
            try
            {
                if (!e.Cancelled)
                {
                    _xml = XElement.Parse(e.Result);

                    WebsiteFeed.Clear();
                    //Results.Items.Clear();
                    foreach (XElement value in _xml.Elements("channel").Elements("item"))
                    {
                        RssViewModel _item = new RssViewModel();
                        _item.Title = value.Element("title").Value;
                        _item.Description = Regex.Replace(value.Element("description").Value,
                        @"<(.|\n)*?>", String.Empty);
                        _item.Link = value.Element("link").Value;
                        _item.Guid = value.Element("guid").Value;
                        _item.Published = DateTime.Parse(value.Element("pubDate").Value);

                        DebugUtils.Log("trololo", string.Format("item : name:{0}", _item.Title));
                        WebsiteFeed.Add(_item);
                        //Results.Items.Add(_item);
                    }
                }
            }
            catch
            {
                // Ignore Errors
            }
        }

        #endregion

        #region Twitter

        public void LoadTweets()
        {
            WebClient clientTweet = new WebClient();
            clientTweet.DownloadStringCompleted += WeblientTwitter_DownloadStringCompleted;
            //clientTweet.DownloadStringAsync(new Uri("https://api.twitter.com/1/statuses/user_timeline.json?trim_user=true&screen_name=roller_station&count=20"));
            clientTweet.DownloadStringAsync(new Uri(("http://api.twitter.com/1/statuses/user_timeline.xml?screen_name=" + HttpUtility.UrlEncode("Air_Roller"))));

        }

        private void WeblientTwitter_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            XElement _xml;
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            try
            {
                if (!e.Cancelled)
                {
                    _xml = XElement.Parse(e.Result);
                    foreach (XElement value in _xml.Elements("status"))
                    {
                        TweetViewModel _tweet = new TweetViewModel();
                        _tweet.Avatar = value.Element("user").Element("profile_image_url").Value;
                        _tweet.Title = value.Element("text").Value;
                        _tweet.Published = DateTime.ParseExact(value.Element("created_at").Value, "ddd MMM dd HH:mm:ss zzzzz yyyy", provider);
                        _tweet.Author = value.Element("user").Element("screen_name").Value;
                        TwitterFeed.Add(_tweet);
                    }
                }
            }
            catch
            {
                // todo catch exception
            }
        }

        #endregion

        #region facebook

        #endregion
    }
}
