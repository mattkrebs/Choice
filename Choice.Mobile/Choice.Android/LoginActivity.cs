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

            SetContentView(Resource.Layout.LoginView);
            Button btnGplus = (Button)FindViewById<Button>(Resource.Id.btnGplus);
            Button btnFb = (Button)FindViewById<Button>(Resource.Id.btnFb);
          
                Button btnTwitter = (Button)FindViewById<Button>(Resource.Id.btnTwitter);

            foreach (var item in external)
            {
                switch (item.Name)
                {
                    case "Facebook":
                        btnFb.Visibility = ViewStates.Visible;
                        btnFb.Touch += (s, e) =>
                        {
                            if (e.Event.Action == MotionEventActions.Up)
                            {
                                var bactivity = new Intent(this, typeof(BrowserLoginActivity));
                                bactivity.PutExtra("AuthUrl", item.Url);
                                StartActivity(bactivity);
                            }

                        };
                        break;
                    default:
                        break;
                }
            }







            // Create your application here
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }
    }
}