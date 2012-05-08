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
using Facebook;
using Hammock.Authentication.OAuth;
using Hammock;
using Helpers;
using DnevnikClient.Helpers;

namespace DnevnikClient
{
    public partial class Sharing : PhoneApplicationPage
    {
        private string link, title;

        public Sharing()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigationContext.QueryString.TryGetValue("link", out link);
            NavigationContext.QueryString.TryGetValue("title", out title);
            link = HttpUtility.HtmlDecode(link);
            title = HttpUtility.HtmlDecode(title);
            

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem li = listSharing.SelectedItem as ListBoxItem;
            if (li != null)
            {
                switch (li.Content.ToString())
                {
                    case "sms":
                        Microsoft.Phone.Tasks.SmsComposeTask sms = new Microsoft.Phone.Tasks.SmsComposeTask();
                        sms.Body = title + " - " + link;
                        sms.Show();
                        break;
                    case "email":
                        Microsoft.Phone.Tasks.EmailComposeTask email = new Microsoft.Phone.Tasks.EmailComposeTask();
                        email.Subject = title;
                        email.Body = "Read all about it" +  link;
                        email.Show();
                        break;
                    case "facebook":
                        LoginToFacebook();
                        break;
                    case "twitter":
                        GetTwitterToken();
                        break;
                    default:
                        break;
                }
            }

            listSharing.SelectedIndex = -1;
        }

        #region Facebook
        

        
        private readonly string[] _extendedPermissions = new[] { "publish_stream" };

        private bool _loggedIn = false;

        private FacebookClient _fbClient;

        private void LoginToFacebook()
        {
            browserAuth.IsScriptEnabled = true;
            browserAuth.Navigated -= Twitter_Navigated;

            browserAuth.Navigated += FacebookLoginBrowser_Navigated;


            //InfoPanel.Visibility = Visibility.Collapsed;

            progressMain.IsIndeterminate = true;

            var loginParameters = new Dictionary<string, object>
                                      {
                                          { "response_type", "token" },
                                          { "display", "popup" } // by default for wp7 builds only (in Facebook.dll), display is set to touch.
                                      };

            var navigateUrl = FacebookOAuthClient.GetLoginUrl(FacebookSettings.AppId, null, _extendedPermissions, loginParameters);

            browserAuth.Navigate(navigateUrl);
        }

        private void FacebookLoginBrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            //FacebookOAuthResult.TryParse()
            FacebookOAuthResult oauthResult;
            if (FacebookOAuthResult.TryParse(e.Uri, out oauthResult))
            {
                if (oauthResult.IsSuccess)
                {
                    _fbClient = new FacebookClient(oauthResult.AccessToken);
                    _loggedIn = true;
                    ShareToFacebook();
                }
                else
                {
                    listSharing.Visibility = System.Windows.Visibility.Visible;
                    browserAuth.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                listSharing.Visibility = System.Windows.Visibility.Collapsed;
                browserAuth.Visibility = Visibility.Visible;
            }
            progressMain.IsIndeterminate = false;
        }

        private void ShareToFacebook()
        {
            progressMain.IsIndeterminate = true;

            browserAuth.Visibility = System.Windows.Visibility.Collapsed;
            listSharing.Visibility = System.Windows.Visibility.Visible;

            //var fbApp = new FacebookClient("181774675203310", "c339fc001c57b43327de590b5f5b2a28");

            //Facebook.FacebookOAuthClient fb = new FacebookOAuthClient()

            var parameters = new Dictionary<string, object>
                 {
                     //{"description", title},
                     {"link", link},
                     //{"name", title}
                 };
            _fbClient.PostCompleted += new EventHandler<FacebookApiEventArgs>(fbApp_PostCompleted);
            _fbClient.PostAsync("me/feed", parameters);
            //Api fb = new Api()


        }

        void fbApp_PostCompleted(object sender, FacebookApiEventArgs e)
        {
            //e.
            Dispatcher.BeginInvoke(() =>
            {
                progressMain.IsIndeterminate = false;
                MessageBox.Show("You have succesfully posted link to your profile.");

                browserAuth.Navigated -= FacebookLoginBrowser_Navigated;
            });
        }

        #endregion

        private void browserAuth_Navigating(object sender, NavigatingEventArgs e)
        {
            progressMain.IsIndeterminate = true;
        }



        #region Twitter

        private string _oAuthTokenSecret;
        private string _oAuthToken;

        private void GetTwitterToken()
        {

            progressMain.IsIndeterminate = true;


            var twitterSettings = TwitterHelper.LoadSetting<TwitterAccess>("TwitterAccess");
            if ((twitterSettings != null &&
                !String.IsNullOrEmpty(twitterSettings.AccessToken) &&
                !String.IsNullOrEmpty(twitterSettings.AccessTokenSecret)))
            {
                PostTweet(link, twitterSettings);
                return;
            }

            browserAuth.IsScriptEnabled = false;
            browserAuth.Navigated -= FacebookLoginBrowser_Navigated;

            browserAuth.Navigated += Twitter_Navigated;

            
            //InfoPanel.Visibility = Visibility.Collapsed;

            

            var credentials = new OAuthCredentials
            {
                Type = OAuthType.RequestToken,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                ConsumerKey = TwitterSettings.ConsumerKey,
                ConsumerSecret = TwitterSettings.ConsumerKeySecret,
                Version = TwitterSettings.OAuthVersion,
                CallbackUrl = TwitterSettings.CallbackUri,

            };

            var client = new RestClient
            {
                Authority = "https://api.twitter.com/oauth",
                Credentials = credentials,
                HasElevatedPermissions = true
            };

            var request = new RestRequest
            {
                Path = "/request_token"
            };
            client.BeginRequest(request, new RestCallback(TwitterRequestTokenCompleted));
        }

        private void TwitterRequestTokenCompleted(RestRequest request, RestResponse response, object userstate)
        {
            _oAuthToken = GetQueryParameter(response.Content, "oauth_token");
            _oAuthTokenSecret = GetQueryParameter(response.Content, "oauth_token_secret");
            var authorizeUrl = TwitterSettings.AuthorizeUri + "?oauth_token=" + _oAuthToken;

            if (String.IsNullOrEmpty(_oAuthToken) || String.IsNullOrEmpty(_oAuthTokenSecret))
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("There was an error accessing Twitter. Please try again later."));
                return;
            }

            Dispatcher.BeginInvoke(() => browserAuth.Navigate(new Uri(authorizeUrl)));
        }

        private static string GetQueryParameter(string input, string parameterName)
        {
            foreach (string item in input.Split('&'))
            {
                var parts = item.Split('=');
                if (parts[0] == parameterName)
                {
                    return parts[1];
                }
            }
            return String.Empty;
        }



        private void Twitter_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            progressMain.IsIndeterminate = false;
            listSharing.Visibility = System.Windows.Visibility.Collapsed;
            browserAuth.Visibility = Visibility.Visible;

            //ProgressBar.IsIndeterminate = true;
            //ProgressBar.Visibility = Visibility.Visible;

            //if (e.Uri.AbsoluteUri.CompareTo("https://api.twitter.com/oauth/authorize") == 0)
            //{
            //    ProgressBar.IsIndeterminate = true;
            //    ProgressBar.Visibility = Visibility.Visible;
            //}

            if (e.Uri.AbsoluteUri.Contains(TwitterSettings.CallbackUri))
            {
                var arguments = e.Uri.AbsoluteUri.Split('?');
                if (arguments.Length >= 1)
                {
                    GetAccessToken(arguments[1]);
                }
            }
        }

        private void GetAccessToken(string uri)
        {
            progressMain.IsIndeterminate = true;

            browserAuth.Visibility = System.Windows.Visibility.Collapsed;
            listSharing.Visibility = System.Windows.Visibility.Visible;

            var requestToken = GetQueryParameter(uri, "oauth_token");
            if (requestToken != _oAuthToken)
            {
                MessageBox.Show("Authentication error.");
            }

            var requestVerifier = GetQueryParameter(uri, "oauth_verifier");

            var credentials = new OAuthCredentials
            {
                Type = OAuthType.AccessToken,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                ConsumerKey = TwitterSettings.ConsumerKey,
                ConsumerSecret = TwitterSettings.ConsumerKeySecret,
                Token = _oAuthToken,
                TokenSecret = _oAuthTokenSecret,
                Verifier = requestVerifier
            };

            var client = new RestClient
            {
                Authority = "https://api.twitter.com/oauth",
                Credentials = credentials,
                HasElevatedPermissions = true
            };

            var request = new RestRequest
            {
                Path = "/access_token"
            };

            client.BeginRequest(request, new RestCallback(RequestAccessTokenCompleted));
        }

        private void RequestAccessTokenCompleted(RestRequest request, RestResponse response, object userstate)
        {
            var twitteruser = new TwitterAccess
            {
                AccessToken = GetQueryParameter(response.Content, "oauth_token"),
                AccessTokenSecret = GetQueryParameter(response.Content, "oauth_token_secret"),
                UserId = GetQueryParameter(response.Content, "user_id"),
                ScreenName = GetQueryParameter(response.Content, "screen_name")
            };

            if (String.IsNullOrEmpty(twitteruser.AccessToken) || String.IsNullOrEmpty(twitteruser.AccessTokenSecret))
            {
                Dispatcher.BeginInvoke(() => MessageBox.Show("Authentication error."));
                return;
            }

            TwitterHelper.SaveSetting("TwitterAccess", twitteruser);

            PostTweet(link, twitteruser);
            

            
        }


        private void PostTweet(string tweet, TwitterAccess twitteruser)
        {
            if (String.IsNullOrEmpty(tweet))
                return;


            var twitter = new TwitterHelper(twitteruser);

            // Successful event handler, navigate back if successful
            twitter.TweetCompletedEvent += (sender, e) =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    progressMain.IsIndeterminate = false;
                    MessageBox.Show("You have successfuly posted to your Twitter profile.");
                    browserAuth.Navigated -= Twitter_Navigated;
                });

                

                //if (NavigationService.CanGoBack)
                //    NavigationService.GoBack();
            };

            // Failed event handler, show error
            twitter.ErrorEvent += (sender, e) =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    progressMain.IsIndeterminate = false;
                    MessageBox.Show("There was an error. Please try again later.");
                    browserAuth.Navigated -= Twitter_Navigated;
                });
            };

            twitter.NewTweet(tweet);
        }


        #endregion




    }
}