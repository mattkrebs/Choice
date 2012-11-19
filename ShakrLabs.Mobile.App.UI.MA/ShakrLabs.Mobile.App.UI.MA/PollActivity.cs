using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using ShakrLabs.Mobile.App.Shared.Presenter;
using System.Net;
using Android.Graphics;
using ShakrLabs.Mobile.App.Data.Models;
using Android.Views.Animations;
using ShakrLabs.Mobile.App.Data.Providers;
using System.Collections.Generic;
using ShakrLabs.Mobile.App.Data.ViewModels;

namespace ShakrLabs.Mobile.App.UI.MA
{
    [Activity(Label = "Choice")]
    public class PollActivity : Activity
    {
        int count = 1;
        ChoicePresenter _presenter = new ChoicePresenter();
        PollFragment fragment1;
        PollFragment fragment2;
        ViewFlipper vFlipper;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.RequestWindowFeature(WindowFeatures.IndeterminateProgress);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ChooserView);
         
            // Get our button from the layout resource,
            // and attach an event to it
            ChoiceViewModel polls = ChoicePresenter.Current.GetChoice();

             

            //ImageView button = FindViewById<ImageView>(Resource.Id.imgChoice1);
            vFlipper= this.FindViewById<ViewFlipper>(Resource.Id.viewFlipper1);
           
            fragment1 = FragmentManager.FindFragmentById(Resource.Id.poll_fragment) as PollFragment;
            fragment2 = FragmentManager.FindFragmentById(Resource.Id.poll_fragment2) as PollFragment;

            fragment1.SetPoll(ChoicePresenter.Current.GetChoice());
            fragment2.SetPoll(ChoicePresenter.Current.GetChoice());

            vFlipper.SetInAnimation(this, Android.Resource.Animation.SlideInLeft);
            vFlipper.SetOutAnimation(this, Android.Resource.Animation.SlideOutRight);
         

          
        }
        public void Flip()
        {
            if (vFlipper.CurrentView.Id == Resource.Id.poll_fragment2)
            {
                fragment2.SetPoll(ChoicePresenter.Current.GetChoice());
            }
            else
            {
                fragment1.SetPoll(ChoicePresenter.Current.GetChoice());
            }
            
            vFlipper.ShowNext();
        }

       
        


    }
}

