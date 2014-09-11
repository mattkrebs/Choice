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
using Android.Graphics.Drawables;
using Android.Graphics;
using Choice.Services.Shared.ViewModels;
using FragmentManager = Android.Support.V4.App.FragmentManager;
using Fragment = Android.Support.V4.App.Fragment;
using PicassoSharp;
using System.Threading.Tasks;

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


        public ChoiceItemViewModel CurrentPoll { get; set; }
        public bool Selected = false;

        public ChoiceFragment(ChoiceItemViewModel choice)
        {
            CurrentPoll = choice;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View v = inflater.Inflate(Resource.Layout.ChoiceFragment, null);
            image1 = v.FindViewById<ImageView>(Resource.Id.imgChoice1);
            image1.Tag= CurrentPoll.OptionId1;

            image1.Touch += ImageTouch;
          //  image1.Touch += image1_Touch;
            image2 = v.FindViewById<ImageView>(Resource.Id.imgChoice2);
            image2.Touch += ImageTouch;



            txtScore1 = v.FindViewById<TextView>(Resource.Id.txtScore1);
            txtScore1.Text = String.Format("{0}%", CurrentPoll.Option1Percentage);
            txtScore2 = v.FindViewById<TextView>(Resource.Id.txtScore2);
            txtScore2.Text = String.Format("{0}%", CurrentPoll.Option2Percentage);
            if (!String.IsNullOrEmpty(CurrentPoll.SelectedOptionId))
            {
                txtScore1.Visibility = ViewStates.Gone;
                txtScore2.Visibility = ViewStates.Gone;
            }
            
            Picasso.With(this.Activity).Load(CurrentPoll.ImageUrl1).Into(image1);
            Picasso.With(this.Activity).Load(CurrentPoll.ImageUrl2).Into(image2);

            return v;
        }

        async void ImageTouch(object sender, View.TouchEventArgs e)
        {
            var optionId = ((ImageView)sender).Tag;

            if (e.Event.Action == MotionEventActions.Up)
            {
                Toast.MakeText(this.Activity, optionId + " Was Selected", ToastLength.Short).Show();
                txtScore1.Visibility = ViewStates.Visible;
                txtScore2.Visibility = ViewStates.Visible;
                await Task.Delay(2000);

                ((MainActivity)this.Activity).NextPage();
            }
        }



       
        public void SetPoll(ChoiceItemViewModel poll)
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