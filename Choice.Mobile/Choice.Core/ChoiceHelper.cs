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

namespace Choice.Core
{
    public static class ChoiceHelper
    {
        /// <summary>
        /// Generates Image Name
        /// </summary>
        /// <returns>Image Name with path</returns>
        public static string GenerateImageName()
        {
            string JPEG_FILE_PREFIX = "CHOICE";
            string JPEG_FILE_SUFFIX = ".jpg";
            // Create an image file name
            String timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            String imageFileName = JPEG_FILE_PREFIX + timeStamp + JPEG_FILE_SUFFIX;

            return imageFileName;
        }

    }
}