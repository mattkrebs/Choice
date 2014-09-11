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
using System.IO;
using Choice.Services.Shared.ViewModels;

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
        ChoiceItemViewModel viewModel = new ChoiceItemViewModel();
        private Bitmap bitmap;

        public const int SAVECHOICE = 0;        
        public const int SELECTIONTYPE = 1;
        public int currentImage = 1;

        public const int PHOTO_CHOICE_ONE = 1;
        public const int PHOTO_CHOICE_TWO = 2;
     

        public string AlbumDirectory = "Choice";
		List<string> cats = new List<string>{"lolcats", "random", "meme"};
       

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.NewChoiceView);
			
            _choiceImage1 = this.FindViewById<ImageView>(Resource.Id.imgChoice1);           
            _choiceImage2 = this.FindViewById<ImageView>(Resource.Id.imgChoice2);
            
            _btnSave = this.FindViewById<Button>(Resource.Id.btnSave);
            _btnSave.Click += (sender, e) =>
            {
                ShowDialog(SAVECHOICE);
            };
            _cameraButton1 = this.FindViewById<ImageView>(Resource.Id.cameraButton1);
            _cameraButton2 = this.FindViewById<ImageView>(Resource.Id.cameraButton2);

            _cameraButton1.Click += (sender, e)=>{
                currentImage = 1;
                ShowDialog(SELECTIONTYPE);
            };
            _cameraButton2.Click += (sender, e) =>
            {
                currentImage = 2;
                ShowDialog(SELECTIONTYPE);
            };

            Cleanup();

            // Create your application here
        }

        protected override Dialog OnCreateDialog(int id)
        {
            var builder = new AlertDialog.Builder(this);
          
            switch (id)
            {
                case SAVECHOICE:
                    builder.SetTitle("Select Category for your Choice");
                    builder.SetItems(cats.ToArray(), ItemSelected);                    
                    builder.SetNegativeButton("Cancel", Cancel);
                    break;               
                case SELECTIONTYPE:
                     builder.SetTitle("Image Source");
                    builder.SetPositiveButton("Take A Photo", TakePhoto);
                    builder.SetNegativeButton("Select From Gallery", SelectFromGallery);
                    break;
               
            }
            return builder.Show();
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

                RunOnUiThread(() => ShowImage(currentImage, t.Result.Path));
            });
        }
        
       
       
        private void TakePhoto(object sender, DialogClickEventArgs e)
        {
           // DispatchTakePictureEvent(currentImage, ChoiceHelper.GenerateImageName());
        }
        private void ShowImage(int image, string path)
        {
            //if (viewModel.Choice.Images == null)
            //    viewModel.Choice.Images = new List<ChoiceImage>();

            //if (image == PHOTO_CHOICE_ONE)
            //{
            //    ChoiceImage ci = new ChoiceImage(){ ImageUrl=  path};
            //    DecodeBitmapAsync(path, 400, 400).ContinueWith(t =>
            //    {
            //        this._choiceImage1.SetImageBitmap(this.bitmap = t.Result);                    
            //        _cameraButton1.Visibility = ViewStates.Gone;
            //        using (var stream = new MemoryStream()){
            //            bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100,stream);
            //            ci.ImageStream = stream.ToArray();
            //        }

            //    }, TaskScheduler.FromCurrentSynchronizationContext());
            //    viewModel.Choice.Images.Add(ci);
            //}
            //else
            //{
            //    ChoiceImage ci = new ChoiceImage() { ImageUrl = path };
            //    DecodeBitmapAsync(path, 400, 400).ContinueWith(t =>
            //    {
            //        this._choiceImage2.SetImageBitmap(this.bitmap = t.Result);
            //        using (var stream = new MemoryStream()){
            //            bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100,stream);
            //            ci.ImageStream = stream.ToArray();
            //        }
            //        _cameraButton1.Visibility = ViewStates.Gone;

            //    }, TaskScheduler.FromCurrentSynchronizationContext());
            //    viewModel.Choice.Images.Add(ci);
            //}

            
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

        private void Cancel(object sender, DialogClickEventArgs e)
        {
            RemoveDialog(e.Which);
        }

        private void SaveChoice()
        {
            //if (viewModel.Choice.Images.Count == 2)
            //{
                
            //    viewModel.SaveChoiceAsync();
            //}
            //else
            //{
            //    Toast.MakeText(this, "Please Select 2 images before saving", ToastLength.Long);
            //}
            
        }

        private void ItemSavedEvent()
        {
            Toast.MakeText(this, "Saved", ToastLength.Long).Show();
        }

        private void ItemSelected(object sender, DialogClickEventArgs e)
        {
			//viewModel.Choice.Category = cats[e.Which];

			SaveChoice();
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