using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Choice.Android
{
    [Activity(Label = "Choice", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : BaseActivity
    {
       
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

          
        }
    }
}

