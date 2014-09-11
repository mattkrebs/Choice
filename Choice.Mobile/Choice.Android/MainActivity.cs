using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using Android.Support.V4.View;
using System.Collections.Generic;
using Android.Support.V4.App;

using FragmentManager = Android.Support.V4.App.FragmentManager;
using Fragment = Android.Support.V4.App.Fragment;
using Choice.Services.Shared.ViewModels;
using Choice.Services.Shared.Services;
using Android.Views.Animations;

namespace Choice.Android
{
    [Activity(Label = "Choice", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : FragmentActivity
    {

        public static List<ChoiceItemViewModel> Choices { get; set; }
        

        public ViewPager ChoicePager;
       
    
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            if (!ChoiceServices.Instance.LoggedIn)
            {
                this.StartActivity(typeof(LoginActivity));
            }
            else
            {

                //load choices
                await ChoiceListViewModel.Current.GetChoicesAsync();

                // Set our view from the "main" layout resource
                SetContentView(Resource.Layout.Main);
                ChoicePager = FindViewById<ViewPager>(Resource.Id.imagePager);


                ChoicePager.SetPageTransformer(true, new ZoomOutPageTransformer());
                ChoicePager.Adapter = new ImageFragementAdapter(SupportFragmentManager);
            }
        }

       
  
       
        void CreateAndShowDialog(Exception exception, String title)
        {
            CreateAndShowDialog(exception.Message, title);
        }

        void CreateAndShowDialog(string message, string title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }

        internal void NextPage()
        {
            ChoicePager.SetCurrentItem(ChoiceListViewModel.Current.CurrentPage + 1, true);
        }
    }

    public class ImageFragementAdapter : FragmentPagerAdapter
    {

        //swich to FragemntStatePagerAdapter
        public ImageFragementAdapter(FragmentManager fm)
            : base(fm)
        {

        }

        public override Fragment GetItem(int position)
        {
            ChoiceListViewModel.Current.CurrentPage = position;
            return new ChoiceFragment(ChoiceListViewModel.Current.Choices[position]);
        }

        public override int Count
        {
            get { return ChoiceListViewModel.Current.Choices.Count; }
        }

    }

    public class ZoomOutPageTransformer : Java.Lang.Object, ViewPager.IPageTransformer
    {
        private static float MIN_SCALE = 0.85f;
        private static float MIN_ALPHA = 0.5f;


        public void TransformPage(View page, float position)
        {
            if (position < -1)
            {
                page.Alpha = 0;

            }
            else if (position <= 1)
            {
                float scaleFactor = Math.Max(MIN_SCALE, 1 - Math.Abs(position));
                float vertMargin = page.Height * (1 - scaleFactor) / 2;
                float horzMargin = page.Width * (1 - scaleFactor) / 2;
                if (position < 0)
                {
                    page.TranslationX = (horzMargin - vertMargin / 2);
                }
                else
                {
                    page.TranslationX = (-horzMargin + vertMargin / 2);
                }

                // Scale the page down (between MIN_SCALE and 1)
                page.ScaleX = scaleFactor;
                page.ScaleY = scaleFactor;


                // Fade the page relative to its size.
                page.Alpha = (MIN_ALPHA + (scaleFactor - MIN_SCALE) / (1 - MIN_SCALE) * (1 - MIN_ALPHA));

            }
            else
            { // (1,+Infinity]
                // This page is way off-screen to the right.
                page.Alpha = 0;
            }
        }

        public IntPtr Handle
        {
            get { return ((Java.Lang.Object)this).Handle; }
        }

        public void Dispose()
        {
            ((Java.Lang.Object)this).Dispose();
        }
    }

    

}

