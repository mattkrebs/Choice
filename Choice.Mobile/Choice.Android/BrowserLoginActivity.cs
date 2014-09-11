using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Webkit;
using Choice.Services.Shared.Services;

namespace Choice.Android
{
    [Activity(Label = "Login")]
    public class BrowserLoginActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.BrowserLoginView);

            var webView = (WebView)this.FindViewById<WebView>(Resource.Id.webView);           
           // var webView = new WebView(this);

            webView.SetWebViewClient(new LoginWebViewClient());
            webView.LoadUrl("http://choice.azurewebsites.net"+ ChoiceServices.Instance.ExternalLogins[0].Url);

        }
    }

    public class LoginWebViewClient : WebViewClient
    {
        public override void OnPageFinished(WebView view, string url)
        {
            base.OnPageFinished(view, url);

            Console.WriteLine(url);
            ParseUrlForAccessToken(url);
        }


        public bool IsLocalUser(string url)
        {
            var cookies = CookieManager.Instance.GetCookie(url);

            Console.WriteLine("All Cookies " + cookies);

            return cookies.Contains(".AspNet.Cookies");            
        }

        private async void ParseUrlForAccessToken(string url)
        {
            const string fieldName = "access_token=";
            int accessTokenIndex = url.IndexOf(fieldName, StringComparison.Ordinal);
            if (accessTokenIndex > -1)
            {

                int ampersandTokenIndex = url.IndexOf("&", accessTokenIndex, StringComparison.Ordinal);
                string tokenField = url.Substring(accessTokenIndex, ampersandTokenIndex - accessTokenIndex);
                string token = tokenField.Substring(fieldName.Length);
                Console.WriteLine(token);
                ChoiceServices.Instance.AccessToken = token;

                if (!IsLocalUser(url))
                {
                    //TODO, collect Email
                    await ChoiceServices.Instance.RegisterExternal("krebs44@gmail.com", token);
                }
                else
                {
                    //Congrats...you are authorized
                    Console.WriteLine("Congrats...you are authorized");
                }
                
                
            }
        }
    }
}