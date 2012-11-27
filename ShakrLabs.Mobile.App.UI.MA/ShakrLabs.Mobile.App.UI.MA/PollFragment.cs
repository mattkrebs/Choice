using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using ShakrLabs.Mobile.App.Data.Models;
using Java.Lang;
using Android.Graphics.Drawables;
using ShakrLabs.Mobile.App.Data.ViewModels;

namespace ShakrLabs.Mobile.App.UI.MA
{
    public class PollFragment : Fragment
    {
        ImageView image1;
        ImageView image2;
        TextView txtScore1;
        TextView txtScore2;
        bool image1loaded = false;
        bool image2loaded = false;
        private PollRunnable pollRunnable;
        public ChoiceViewModel CurrentPoll { get; set; }
        public bool Selected = false;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.ChoiceFragment, null);
            image1 = v.FindViewById<ImageView>(Resource.Id.imgChoice1);
            var animation = (AnimationDrawable)image1.Drawable;
            animation.Start();
            
            image1.Touch += image1_Touch;
            image2 = v.FindViewById<ImageView>(Resource.Id.imgChoice2);
            txtScore1 = v.FindViewById<TextView>(Resource.Id.txtScore1);
            txtScore1.Text = "40%";
            txtScore2 = v.FindViewById<TextView>(Resource.Id.txtScore2);
            txtScore2.Text = "60%";
            txtScore1.Visibility = ViewStates.Gone;
            txtScore2.Visibility = ViewStates.Gone;
            image2.Touch += image2_Touch;
           
            pollRunnable = new PollRunnable(this);
 
           


            return v;
        }

        void image1_Touch(object sender, View.TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Up && Selected == false)
            {
                Selected = true;
                Handler handler = new Handler();
                txtScore1.Visibility = ViewStates.Visible;
                // Toast.MakeText(this.Activity, "Clicked on image :" + CurrentPoll.PollItems[0].ImageUrl, ToastLength.Short).Show();
                handler.PostDelayed(pollRunnable, 2000);
            }
        }
        void image2_Touch(object sender, View.TouchEventArgs e)
        {
            if (e.Event.Action == MotionEventActions.Up && Selected == false)
            {
                Selected = true;
                Handler handler = new Handler();
                txtScore2.Visibility = ViewStates.Visible;

                // Toast.MakeText(this.Activity, "Clicked on image :" + CurrentPoll.PollItems[1].ImageUrl, ToastLength.Short).Show();
                handler.PostDelayed(pollRunnable, 2000);
            }
        }

     
        private class PollRunnable : Java.Lang.Object, IRunnable
        {
            private PollFragment item;
            public PollRunnable(PollFragment frag)
            {
                this.item = frag;
            }
            public void Run()
            {
                item.txtScore1.Visibility = ViewStates.Gone;
                item.txtScore2.Visibility = ViewStates.Gone;
                item.Selected = false;
                ((ChoiceActivity)this.item.Activity).Flip();
            }
        }
        public void SetPoll(ChoiceViewModel poll)
        {
            CurrentPoll = poll;
            this.Activity.SetProgressBarIndeterminateVisibility(true);
            WebClient web = new WebClient();
           
                web.DownloadDataCompleted += new DownloadDataCompletedEventHandler(web_DownloadDataCompleted);
                web.DownloadDataAsync(new Uri(poll.ImageUrl1));


                WebClient web1 = new WebClient();
                web1.DownloadDataCompleted += new DownloadDataCompletedEventHandler(web_DownloadDataCompleted2);
                web1.DownloadDataAsync(new Uri(poll.ImageUrl2));
          
          

        }

        public void ImageLoaded()
        {
            if (image2loaded && image1loaded)
            {
                this.Activity.SetProgressBarIndeterminateVisibility(false);
            }
        }
      
        public void web_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {

            if (e.Error != null)
            {
                Activity.RunOnUiThread(() =>
                {
                    Toast.MakeText(this.Activity, "Error Downloading Image 1" + e.Error.Message, ToastLength.Long).Show();
                    Log.Warn("Downloading Image", e.Error.InnerException.Message);
                });
            }
            else
            {

                Bitmap bm = BitmapFactory.DecodeByteArray(e.Result, 0, e.Result.Length);
              
                Activity.RunOnUiThread(() =>
                {
                    image1loaded = true;
                    image1.SetImageBitmap(bm);
                      Log.Debug("Downloading Image", "Image slot 1");
                });
            }
        }
        public void web_DownloadDataCompleted2(object sender, DownloadDataCompletedEventArgs e)
        {

            if (e.Error != null)
            {
                Activity.RunOnUiThread(() =>
                {
                    Toast.MakeText(this.Activity, "Error Downloading Image 1" + e.Error.Message, ToastLength.Long).Show();
                    Log.Warn("Downloading Image", e.Error.InnerException.Message);
                });
                
            }
            else
            {

                Bitmap bm = BitmapFactory.DecodeByteArray(e.Result, 0, e.Result.Length);

                Activity.RunOnUiThread(() =>
                {
                    image2loaded = true;
                    image2.SetImageBitmap(bm);

                    Log.Debug("Downloading Image", "Image slot 1");
                });
            }
        }
     
    
    }
}