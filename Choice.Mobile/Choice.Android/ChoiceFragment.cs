using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Choice.Core;
using Android.Graphics.Drawables;
using Android.Graphics;

namespace Choice.Android
{
    public class ChoiceFragment : Fragment
    {
        ImageView image1;
        ImageView image2;
        TextView txtScore1;
        TextView txtScore2;
        bool image1loaded = false;
        bool image2loaded = false;


        public ChoiceViewModel CurrentPoll { get; set; }
        public bool Selected = false;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.ChoiceFragment, null);
            image1 = v.FindViewById<ImageView>(Resource.Id.imgChoice1);    
          //  image1.Touch += image1_Touch;
            image2 = v.FindViewById<ImageView>(Resource.Id.imgChoice2);
            txtScore1 = v.FindViewById<TextView>(Resource.Id.txtScore1);
            txtScore1.Text = "40%";
            txtScore2 = v.FindViewById<TextView>(Resource.Id.txtScore2);
            txtScore2.Text = "60%";
            txtScore1.Visibility = ViewStates.Gone;
            txtScore2.Visibility = ViewStates.Gone;
          //  image2.Touch += image2_Touch;
			
            return v;
        }

       


       
        public void SetPoll(ChoiceViewModel poll)
        {
            CurrentPoll = poll;
            this.Activity.SetProgressBarIndeterminateVisibility(true);
          



        }

        public void ImageLoaded()
        {
            if (image2loaded && image1loaded)
            {
                this.Activity.SetProgressBarIndeterminateVisibility(false);
            }
        }

     
    
    }
}