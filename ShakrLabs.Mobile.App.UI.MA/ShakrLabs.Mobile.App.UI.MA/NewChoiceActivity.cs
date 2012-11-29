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
using Android.Net;
using Java.IO;
using Android.Net;
using ShakrLabs.Mobile.App.Shared.Presenter;
using ShakrLabs.Mobile.App.Data.ViewModels;

namespace ShakrLabs.Mobile.App.UI.MA
{
    [Activity(Label = "New Choice", ScreenOrientation= Android.Content.PM.ScreenOrientation.Portrait)]
    public class NewChoiceActivity : BaseActivity
    {
        private ImageView _choiceImage1;
        private ImageView _choiceImage2;
        private ImageView _cameraButton1;
        private ImageView _cameraButton2;
        private Button _btnSave;

        public const int SAVECHOICE = 0;
        public const int CHOICEERROR = 1;
        public const int SELECTIONTYPE1 = 2;
        public const int SELECTIONTYPE2 = 3;

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

        protected override Dialog OnCreateDialog(int id)
        {
            var builder = new AlertDialog.Builder(this);
            List<string> cats = new List<string>{"lolcats", "random", "meme"};
            switch (id)
            {
                case SAVECHOICE:
                    builder.SetTitle("Select Category for your Choice");
                    builder.SetItems(cats.ToArray(), ItemSelected);
                    builder.SetPositiveButton("OK", SaveChoice);
                    builder.SetNegativeButton("Cancel", Cancel);
                    break;
                case CHOICEERROR:
                     builder.SetTitle("There Has Been An Error");                    
                    builder.SetPositiveButton("OK", Cancel);
                    builder.SetNegativeButton("Cancel", Cancel);
                    break;
                case SELECTIONTYPE1:
                     builder.SetTitle("Image Source");
                    builder.SetPositiveButton("Take A Photo", TakePhoto);
                    builder.SetNegativeButton("Select From Gallery", SelectFromGallery);
                    break;
                case SELECTIONTYPE2:
                    builder.SetTitle("Image Source");
                    builder.SetPositiveButton("Take A Photo", TakePhoto2);
                    builder.SetNegativeButton("Select From Gallery", SelectFromGallery2);
                    break;
            }
            return builder.Show();
        }

        private void SelectFromGallery2(object sender, DialogClickEventArgs e)
        {
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(imageIntent, "Select photo"), PHOTO_CHOICE_TWO);
        }
        private void SelectFromGallery(object sender, DialogClickEventArgs e)
        {
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(Intent.CreateChooser(imageIntent, "Select photo"), PHOTO_CHOICE_ONE);
        }
        private void TakePhoto2(object sender, DialogClickEventArgs e)
        {
            photoPath2 = createImageFile();
            DispatchTakePictureEvent(PHOTO_CHOICE_TWO, photoPath2);
        }

        private void TakePhoto(object sender, DialogClickEventArgs e)
        {
            photoPath1 = createImageFile();
            DispatchTakePictureEvent(PHOTO_CHOICE_ONE, photoPath1);
        }

        private void Cancel(object sender, DialogClickEventArgs e)
        {
            RemoveDialog(e.Which);
        }

        private void SaveChoice(object sender, DialogClickEventArgs e)
        {
            System.Console.WriteLine("Saving Choice");
        }

        private void ItemSelected(object sender, DialogClickEventArgs e)
        {
            
        }


        void _btnSave_Click(object sender, EventArgs e)
        {
            ShowDialog(SAVECHOICE);
        }

        void _choiceImage2_Click(object sender, EventArgs e)
        {
            ShowDialog(SELECTIONTYPE2);
        }

        void _choiceImage1_Click(object sender, EventArgs e)
        {
            ShowDialog(SELECTIONTYPE1);
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



            if (_file != null)
            {
                var contentUri = Android.Net.Uri.FromFile(_file);
                var bitmap = MediaStore.Images.Media.GetBitmap(ContentResolver, contentUri);
                var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);

                mediaScanIntent.SetData(contentUri);
                this.SendBroadcast(mediaScanIntent);
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
            else if (resultCode == Result.Ok)
            {
                if (requestCode == PHOTO_CHOICE_ONE)
                {
                    _choiceImage1.SetImageURI(intent.Data);
                    _cameraButton1.Visibility = ViewStates.Gone;
                }
                else
                {
                    _choiceImage2.SetImageURI(intent.Data);
                    _cameraButton2.Visibility = ViewStates.Gone;
                }
            }
           
        }

        private void ScanFile(Android.Net.Uri contentUri){
           
        }

        
    }
}