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
using ShakrLabs.Mobile.App.Shared.Presenter;

namespace ShakrLabs.Mobile.App.UI.MA
{
    [Activity(Label = "")]
    public class SplashScreenActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
        }

        protected override void OnResume()
        {
            base.OnResume();

            //Check if user is loged in
            //ChoicePresenter.Current.GetRandomChoicesAsync(BuildUI, "token");
        }
    }
}