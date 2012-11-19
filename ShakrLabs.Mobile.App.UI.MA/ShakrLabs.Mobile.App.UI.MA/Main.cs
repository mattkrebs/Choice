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
using System.IO;
using Android.Util;

namespace ShakrLabs.Mobile.App.UI.MA
{
    [Application]
    public class ChoiceApp : Application
    { 
        public static ChoiceApp Current { get; private set; }

        public ChoiceApp(IntPtr handle, global::Android.Runtime.JniHandleOwnership transfer)
            : base(handle, transfer)
        {
            Current = this;
        }

        public override void OnCreate()
        {
            LogDebug("MAIN OnCreate ---------------------------------");
            base.OnCreate();
            var getAppAssetStringDelegate = new Func<string, string>(GetAppAssetString);
           // SharedApp.SetAppAssetStringDelegate(getAppAssetStringDelegate);
           // SharedApp.OnStart();
        }

        public override void OnTerminate()
        {
            LogDebug("MAIN OnTerminate");
            base.OnTerminate();
        }

        private string GetAppAssetString(string uri)
        {
            string content = null;
            using (var sr = new StreamReader(Assets.Open(uri))) // example "Config/AppConfig.xml"
            {
                content = sr.ReadToEnd();
            }
            return content;
        }

        /*
        Use this to help with ADB watching in CMD 
        "c:\Program Files (x86)\Android\android-sdk\platform-tools\adb" logcat -s MonoDroid:* mono:* MWC:* ActivityManager:*
        */
        public static void LogDebug(string message)
        {
            Console.WriteLine(message);
            Log.Debug("MWC", message);
        }
    }
}