using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Windows;
using Hammock;
using Hammock.Authentication.OAuth;
using Hammock.Web;
using System.IO.IsolatedStorage;
using System.IO;
using System.Runtime.Serialization;
//using Twitt.ViewModels;

namespace Helpers
{
    public class TwitterHelper
    {
        private const String MaxCount = "200";
        private readonly TwitterAccess _twitterSettings;
        private readonly bool _authorized;
        private readonly OAuthCredentials _credentials;
        private readonly RestClient _client;
        public event EventHandler TweetCompletedEvent;
        public event EventHandler LoadedCompleteEvent;
        public event EventHandler FavoriteCompletedEvent;
        public event EventHandler ErrorEvent;

        private static Object _thisLock = new Object();

        public TwitterHelper(TwitterAccess twittersettings)
        {
            _twitterSettings = twittersettings; //Helper.LoadSetting<TwitterAccess>(Constants.TwitterAccess);

            //if (_twitterSettings == null || String.IsNullOrEmpty(_twitterSettings.AccessToken) ||
            //   String.IsNullOrEmpty(_twitterSettings.AccessTokenSecret))
            //{
            //    return;
            //}

            _authorized = true;

            _credentials = new OAuthCredentials
            {
                Type = OAuthType.ProtectedResource,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                ParameterHandling = OAuthParameterHandling.HttpAuthorizationHeader,
                ConsumerKey = TwitterSettings.ConsumerKey,
                ConsumerSecret = TwitterSettings.ConsumerKeySecret,
                Token = _twitterSettings.AccessToken,
                TokenSecret = _twitterSettings.AccessTokenSecret,
                Version = TwitterSettings.OAuthVersion,
            };

            _client = new RestClient
            {
                Authority = "http://api.twitter.com",
                HasElevatedPermissions = true
            };
        }

        public void NewTweet(string tweetText)
        {
            if (!_authorized)
            {
                if (ErrorEvent != null)
                    ErrorEvent(this, EventArgs.Empty);
                return;
            }

            var request = new RestRequest
            {
                Credentials = _credentials,
                Path = "/statuses/update.xml",
                Method = WebMethod.Post
            };

            request.AddParameter("status", tweetText);

            _client.BeginRequest(request, new RestCallback(NewTweetCompleted));
        }

        private void NewTweetCompleted(RestRequest request, RestResponse response, object userstate)
        {
            // We want to ensure we are running on our thread UI
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        if (TweetCompletedEvent != null)
                            TweetCompletedEvent(this, EventArgs.Empty);
                    }
                    else
                    {
                        if (ErrorEvent != null)
                            ErrorEvent(this, EventArgs.Empty);
                    }
                });
        }


        public static T LoadSetting<T>(string fileName)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!store.FileExists(fileName))
                    return default(T);

                lock (_thisLock)
                {
                    try
                    {
                        using (var stream = store.OpenFile(fileName, FileMode.Open, FileAccess.Read))
                        {
                            var serializer = new DataContractSerializer(typeof(T));
                            return (T)serializer.ReadObject(stream);
                        }
                    }
                    catch (SerializationException se)
                    {
                        //Deployment.Current.Dispatcher.BeginInvoke(
                        //    () => MessageBox.Show(String.Format("Serialize file error {0}:{1}", se.Message, fileName)));
                        return default(T);
                    }
                    catch (Exception e)
                    {
                        //Deployment.Current.Dispatcher.BeginInvoke(
                        //    () => MessageBox.Show(String.Format("Load file error {0}:{1}", e.Message, fileName)));
                        return default(T);
                    }
                }
            }
        }

        public static void SaveSetting<T>(string fileName, T dataToSave)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                lock (_thisLock)
                {
                    try
                    {
                        using (var stream = store.CreateFile(fileName))
                        {
                            var serializer = new DataContractSerializer(typeof(T));
                            serializer.WriteObject(stream, dataToSave);
                        }
                    }
                    catch (Exception e)
                    {
                        //MessageBox.Show(String.Format("Save file error {0}:{1}", e.Message, fileName));
                        return;
                    }
                }
            }
        }



    }
}
