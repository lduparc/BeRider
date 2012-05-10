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
using Rider.Models;
using Rider.Persistent;
using Rider.Utils;
using Hammock.Silverlight.Compat;
using Rider.Resources;

namespace Rider.Views
{
    public partial class ShareSelectionPage : PhoneApplicationPage
    {
        private string sport, duration, distance, kcal;

        public ShareSelectionPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            NavigationContext.QueryString.TryGetValue("sport", out sport);
            NavigationContext.QueryString.TryGetValue("duration", out duration);
            NavigationContext.QueryString.TryGetValue("distance", out distance);
            NavigationContext.QueryString.TryGetValue("kcal", out kcal);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem li = listSharing.SelectedItem as ListBoxItem;
            if (li != null)
            {
                switch (li.Content.ToString())
                {
                    case "Sms":
                        Microsoft.Phone.Tasks.SmsComposeTask sms = new Microsoft.Phone.Tasks.SmsComposeTask();
                        sms.Body = AppResource.ResourceManager.GetString("ApplicationName");
                        sms.Show();
                        break;
                    case "Email":
                        Microsoft.Phone.Tasks.EmailComposeTask email = new Microsoft.Phone.Tasks.EmailComposeTask();
                        email.Subject = AppResource.ResourceManager.GetString("ApplicationName");
                        email.Body = AppResource.ResourceManager.GetString("NewSession") + " : " + "\n" + sport;
                        email.Show();
                        break;
                    case "Facebook":
                        LoginToFacebook();
                        break;
                    case "Twitter":
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

            shellProgress.IsVisible = true;
            var loginParameters = new Dictionary<string, object>();
            //loginParameters["client_id"] = "2850619249190833";
loginParameters["response_type"] = "token";
loginParameters["display"] = "popup";
loginParameters["redirect_uri"] = "https://www.facebook.com/connect/login_success.html";

            //var loginParameters = new Dictionary<string, object>
            //                          {
            //                              { "response_type", "token" },
            //                              { "display", "popup" } // by default for wp7 builds only (in Facebook.dll), display is set to touch.
            //                          };

            var navigateUrl = FacebookOAuthClient.GetLoginUrl(Configuration.FacebookAppId, null, _extendedPermissions, loginParameters);

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
            shellProgress.IsVisible = false;
        }

        private void ShareToFacebook()
        {
            shellProgress.IsVisible = true;

            browserAuth.Visibility = System.Windows.Visibility.Collapsed;
            listSharing.Visibility = System.Windows.Visibility.Visible;

            var fbApp = new FacebookClient("285061924919083", "e886b40732096675e339ffff0a78bc7b");

            //Facebook.FacebookOAuthClient fb = new FacebookOAuthClient()

            var parameters = new Dictionary<string, object>
                 {
                     {"message", AppResource.ResourceManager.GetString("NewSession") + "\n" + "trotlerkfx"},
                     {"name", AppResource.ResourceManager.GetString("ApplicationName")}
                 };
            _fbClient.PostCompleted += new EventHandler<FacebookApiEventArgs>(fbApp_PostCompleted);
            _fbClient.PostAsync("me/feed", parameters);
        }

        void fbApp_PostCompleted(object sender, FacebookApiEventArgs e)
        {
            Dispatcher.BeginInvoke(() =>
            {
                shellProgress.IsVisible = false;
                MessageBox.Show("You have succesfully posted link to your profile.");

                browserAuth.Navigated -= FacebookLoginBrowser_Navigated;
            });
        }

        #endregion

        private void browserAuth_Navigating(object sender, NavigatingEventArgs e)
        {
            shellProgress.IsVisible = true;
        }

        #region Twitter

        private string _oAuthTokenSecret;
        private string _oAuthToken;

        private void GetTwitterToken()
        {

            shellProgress.IsVisible = true;


            var twitterSettings = TwitterHelper.LoadSetting<TwitterAccess>("TwitterAccess");
            if ((twitterSettings != null &&
                !String.IsNullOrEmpty(twitterSettings.AccessToken) &&
                !String.IsNullOrEmpty(twitterSettings.AccessTokenSecret)))
            {
                PostTweet(AppResource.ResourceManager.GetString("NewSession"), twitterSettings);
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
                ConsumerKey = Configuration.ConsumerKey,
                ConsumerSecret = Configuration.ConsumerKeySecret,
                Version = Configuration.OAuthVersion,
                CallbackUrl = Configuration.CallbackUri
            };

            //var client = new RestClient
            //{
            //    Authority = "https://api.twitter.com/oauth",
            //    Credentials = credentials,
            //    HasElevatedPermissions = true,
            //    SilverlightAcceptEncodingHeader = "gzip",
            //    DecompressionMethods = DecompressionMethods.GZip
            //};

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
            var authorizeUrl = Configuration.AuthorizeUri + "?oauth_token=" + _oAuthToken;

            if (String.IsNullOrEmpty(_oAuthToken) || String.IsNullOrEmpty(_oAuthTokenSecret))
            {
                Dispatcher.BeginInvoke(() =>
                {
                    shellProgress.IsVisible = false;
                    MessageBox.Show("There was an error accessing Twitter. Please try again later.");
                });
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
            shellProgress.IsVisible = false;
            listSharing.Visibility = System.Windows.Visibility.Collapsed;
            browserAuth.Visibility = Visibility.Visible;

            //ProgressBar.IsIndeterminate = true;
            //ProgressBar.Visibility = Visibility.Visible;

            //if (e.Uri.AbsoluteUri.CompareTo("https://api.twitter.com/oauth/authorize") == 0)
            //{
            //    ProgressBar.IsIndeterminate = true;
            //    ProgressBar.Visibility = Visibility.Visible;
            //}

            if (e.Uri.AbsoluteUri.Contains(Configuration.CallbackUri))
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
            shellProgress.IsVisible = true;

            browserAuth.Visibility = System.Windows.Visibility.Collapsed;
            listSharing.Visibility = System.Windows.Visibility.Visible;

            var requestToken = GetQueryParameter(uri, "oauth_token");
            if (requestToken != _oAuthToken)
            {
                shellProgress.IsVisible = false;
                MessageBox.Show("Authentication error.");
            }

            var requestVerifier = GetQueryParameter(uri, "oauth_verifier");

            var credentials = new OAuthCredentials
            {
                Type = OAuthType.AccessToken,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                ConsumerKey = Configuration.ConsumerKey,
                ConsumerSecret = Configuration.ConsumerKeySecret,
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
                Dispatcher.BeginInvoke(() =>
                {
                    shellProgress.IsVisible = false;
                    MessageBox.Show("Authentication error.");
                });
                return;
            }

            TwitterHelper.SaveSetting("TwitterAccess", twitteruser);

            PostTweet(AppResource.ResourceManager.GetString("NewSession"), twitteruser);



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
                    shellProgress.IsVisible = false;
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
                    shellProgress.IsVisible = false;
                    MessageBox.Show("There was an error. Please try again later.");
                    browserAuth.Navigated -= Twitter_Navigated;
                });
            };

            twitter.NewTweet(tweet);
        }


        #endregion
    }
}