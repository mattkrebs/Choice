using System.Net;

namespace ShakrLabs.Mobile.App.Data.Providers.Base
{
    /// <summary>
    /// Because RemoteDataProvider is a generic type, statics need to be kept elsewhere. This class keeps track of the member service cookies.
    /// </summary>
    internal static class ServiceSessionManager
    {
        /// <summary>
        /// Keeps track of cookies for the member site.
        /// </summary>
        public static CookieContainer MemberCookies { get; set; }

        /// <summary>
        /// Keeps track of cookies for the public site
        /// </summary>
        public static CookieContainer PublicCookies { get; set; }

        /// <summary>
        /// Keeps track of the member URL
        /// </summary>
        public static string Referrer { get; set; }

        /// <summary>
        /// Is the user currently logged on?
        /// </summary>
        public static bool IsLoggedIn
        {
            get
            {
                return _isLoggedIn;
            }
            set
            {
                _isLoggedIn = value;
            }
        }
        /// <summary>
        /// Is the user currently logged on?
        /// </summary>
        private static bool _isLoggedIn;

        /// <summary>
        /// Initializes static members of the <see cref="ServiceSessionManager" /> class.
        /// </summary>
        static ServiceSessionManager()
        {
            MemberCookies = new CookieContainer();
            PublicCookies = new CookieContainer();
        }
    }
}