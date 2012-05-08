namespace Helpers
{
    public class TwitterSettings
    {
        // Make sure you set your own ConsumerKey and Secret from dev.twitter.com  
        public static string ConsumerKey = "B0VQoaGhHnkCKOvY9z1tQ";
        public static string ConsumerKeySecret = "qLHiz7rhzSnNEeIlz0BHdACMvk8Qdc0PwkXrtthW22M"; 
        
        public static string RequestTokenUri = "https://api.twitter.com/oauth/request_token";
        public static string OAuthVersion = "1.0";
        public static string CallbackUri = "http://m.dnevnik.si/";
        public static string AuthorizeUri = "https://api.twitter.com/oauth/authorize";
        public static string AccessTokenUri = "https://api.twitter.com/oauth/access_token";
    }

    public class TwitterAccess
    {
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
        public string UserId { get; set; }
        public string ScreenName { get; set; }
    }
}
