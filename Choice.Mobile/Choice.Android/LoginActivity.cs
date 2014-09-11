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
using Choice.Services.Shared.ViewModels;
using Choice.Services.Shared.Services;
using Xamarin.Auth;
using Android.Webkit;

namespace Choice.Android
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            List<ExternalLoginViewModel> external = await ChoiceServices.Instance.GetExternalLoginProviders();

            ChoiceServices.Instance.ExternalLogins = external;


            StartActivityForResult(typeof(BrowserLoginActivity), 1);





            // Create your application here
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}