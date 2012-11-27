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
using Android.Provider;
using Android.Graphics;
using Java.IO;
using Android.Net;

namespace ShakrLabs.Mobile.App.UI.MA
{
    [Activity(Label = "New Choice")]
    public class NewChoiceActivity : BaseActivity
    {
        private ImageView _choiceImage1;
        private ImageView _choiceImage2;
        private ImageView _cameraButton1;
        private ImageView _cameraButton2;
        private Button _btnSave;

        public const int PHOTO_CHOICE_ONE = 1;
        public const int PHOTO_CHOICE_TWO = 2;
        public const string JPEG_FILE_PREFIX = "CHOICE";
        public const string JPEG_FILE_SUFFIX = ".jpg";

        public string photoPath1;
        public string photoPath2;

        public string AlbumDirectory = "Choice";

        Java.IO.File _file;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.NewChoiceView);

            _choiceImage1 = this.FindViewById<ImageView>(Resource.Id.imgChoice1);
           
            _choiceImage2 = this.FindViewById<ImageView>(Resource.Id.imgChoice2);
            
            _btnSave = this.FindViewById<Button>(Resource.Id.btnSave);
            _btnSave.Click += _btnSave_Click;
            _cameraButton1 = this.FindViewById<ImageView>(Resource.Id.cameraButton1);
            _cameraButton2 = this.FindViewById<ImageView>(Resource.Id.cameraButton2);

            _cameraButton1.Click += _choiceImage1_Click;
            _cameraButton2.Click += _choiceImage2_Click;
           

            // Create your application here
        }

        void _btnSave_Click(object sender, EventArgs e)
        {
            
        }

        void _choiceImage2_Click(object sender, EventArgs e)
        {
            photoPath2 = createImageFile();
            DispatchTakePictureEvent(PHOTO_CHOICE_TWO, photoPath2);
        }

        void _choiceImage1_Click(object sender, EventArgs e)
        {
            photoPath1 = createImageFile();
            DispatchTakePictureEvent(PHOTO_CHOICE_ONE, photoPath1);
        }

       
        private string createImageFile() {
            // Create an image file name
            String timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            String imageFileName = JPEG_FILE_PREFIX + timeStamp + JPEG_FILE_SUFFIX;

            return imageFileName;
        }

        private void DispatchTakePictureEvent(int code, string name)
        {
            var intent = new Intent(MediaStore.ActionImageCapture);

           
            var availableActivities = this.PackageManager.QueryIntentActivities(intent, Android.Content.PM.PackageInfoFlags.MatchDefaultOnly);

            if (availableActivities != null && availableActivities.Count > 0)
            {
                var dir = new Java.IO.File(
                    Android.OS.Environment.GetExternalStoragePublicDirectory(
                    Android.OS.Environment.DirectoryPictures), "ChoiceApp");

                if (!dir.Exists())
                {
                    dir.Mkdirs();
                }

                _file = new Java.IO.File(dir, name);

                intent.PutExtra(MediaStore.ExtraOutput,
                Android.Net.Uri.FromFile(_file));
                StartActivityForResult(intent, code);
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

       

            var mediaScanIntent =
               new Intent(Intent.ActionMediaScannerScanFile);
            var contentUri = Android.Net.Uri.FromFile(_file);
            mediaScanIntent.SetData(contentUri);
            this.SendBroadcast(mediaScanIntent);

            var bitmap = MediaStore.Images.Media.GetBitmap(ContentResolver, contentUri);
            if (requestCode == PHOTO_CHOICE_ONE)
            {
                _choiceImage1.SetImageBitmap(bitmap);
                _cameraButton1.Visibility = ViewStates.Gone;
            }
            else
            {
                _choiceImage2.SetImageBitmap(bitmap);
                _cameraButton2.Visibility = ViewStates.Gone;
            }

            bitmap.Dispose();

           
        }

        
    }
}