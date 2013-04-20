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
using Xamarin.Media;
using System.Threading.Tasks;

using Choice.Core;

namespace Choice.Android
{

    [Activity(Label = "New Choice")]
    public class NewChoiceActivity : BaseActivity
    {
        private ImageView _choiceImage1;
        private ImageView _choiceImage2;
        private ImageView _cameraButton1;
        private ImageView _cameraButton2;
        private Button _btnSave;

        private Bitmap bitmap;

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

            Cleanup();

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
            var picker = new MediaPicker(this);

            if (!picker.PhotosSupported)
            {
                ShowUnsupported();
                return;
            }

            picker.PickPhotoAsync().ContinueWith(t =>
            {
                if (t.IsCanceled)
                    return;

                RunOnUiThread(() => ShowImage(PHOTO_CHOICE_TWO, t.Result.Path));
            });
        }
        private void SelectFromGallery(object sender, DialogClickEventArgs e)
        {
            var picker = new MediaPicker(this);

            if (!picker.PhotosSupported)
            {
                ShowUnsupported();
                return;
            }

            picker.PickPhotoAsync().ContinueWith(t =>
            {
                if (t.IsCanceled)
                    return;

                RunOnUiThread(() => ShowImage(PHOTO_CHOICE_ONE, t.Result.Path));
            });
        }
        private void TakePhoto2(object sender, DialogClickEventArgs e)
        {
            photoPath2 = createImageFile();
            DispatchTakePictureEvent(PHOTO_CHOICE_TWO, photoPath2);
        }
        private Toast unsupportedToast;
        private void ShowUnsupported()
        {
            if (this.unsupportedToast != null)
            {
                this.unsupportedToast.Cancel();
                this.unsupportedToast.Dispose();
            }

            this.unsupportedToast = Toast.MakeText(this, "Your device does not support this feature", ToastLength.Long);
            this.unsupportedToast.Show();
        }
        private void TakePhoto(object sender, DialogClickEventArgs e)
        {
            photoPath1 = createImageFile();
            DispatchTakePictureEvent(PHOTO_CHOICE_ONE, photoPath1);
        }
        private void ShowImage(int image, string path)
        {
            if (image == PHOTO_CHOICE_ONE)
            {

                DecodeBitmapAsync(path, 400, 400).ContinueWith(t =>
                {
                    this._choiceImage1.SetImageBitmap(this.bitmap = t.Result);                    
                    _cameraButton1.Visibility = ViewStates.Gone;
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                DecodeBitmapAsync(path, 400, 400).ContinueWith(t =>
                {
                    this._choiceImage2.SetImageBitmap(this.bitmap = t.Result);
                    _cameraButton1.Visibility = ViewStates.Gone;

                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
        private static Task<Bitmap> DecodeBitmapAsync(string path, int desiredWidth, int desiredHeight)
        {
            return Task.Factory.StartNew(() =>
            {
                BitmapFactory.Options options = new BitmapFactory.Options();
                options.InJustDecodeBounds = true;
                BitmapFactory.DecodeFile(path, options);

                int height = options.OutHeight;
                int width = options.OutWidth;

                int sampleSize = 1;
                if (height > desiredHeight || width > desiredWidth)
                {
                    int heightRatio = (int)Math.Round((float)height / (float)desiredHeight);
                    int widthRatio = (int)Math.Round((float)width / (float)desiredWidth);
                    sampleSize = Math.Min(heightRatio, widthRatio);
                }

                options = new BitmapFactory.Options();
                options.InSampleSize = sampleSize;

                return BitmapFactory.DecodeFile(path, options);
            });
        }

       
        private void Cleanup()
        {
            if (this.bitmap == null)
                return;

            _choiceImage1.SetImageBitmap(null);
            _choiceImage2.SetImageBitmap(null);
            this.bitmap.Dispose();
            this.bitmap = null;
        }

        private void Cancel(object sender, DialogClickEventArgs e)
        {
            RemoveDialog(e.Which);
        }

        private void SaveChoice(object sender, DialogClickEventArgs e)
        {
			ChoiceViewModel viewModel = new ChoiceViewModel();

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
            var picker = new MediaPicker(this);
            if (!picker.IsCameraAvailable || !picker.PhotosSupported)
            {
                ShowUnsupported();
                return;
            }

            picker.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Name = "name",
                Directory = "ChoiceImages"
            })
            .ContinueWith(t =>
            {
                if (t.IsCanceled)
                    return;

                RunOnUiThread(() => ShowImage(code, t.Result.Path));
            });
        }

       

      
        
    }
}