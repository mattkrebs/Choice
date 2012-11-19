using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Android.Util;

namespace ShakrLabs.Mobile.App.Data.Models
{
    public class PollItem
    {
        public Guid PollId { get; set; }
        public Guid PollItemId { get; set; }
        private string _imageUrl;
        public string ImageUrl {
            get
            {
                return _imageUrl;
            }
            set
            {
                _imageUrl = value;
                DownloadImage(value);

            }
        }
        public DateTime CreatedDate { get; set; }
        public bool Active { get; set; }
        
        public Byte[] RawImage { get; set; }

        public void DownloadImage(string url){
            WebClient web = new WebClient();
            web.DownloadDataCompleted += new DownloadDataCompletedEventHandler(web_DownloadDataCompleted);
            web.DownloadDataAsync(new Uri(url));
        }


        public void web_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {

            if (e.Error != null)
            {
                   Log.Warn("Downloading Image", e.Error.Message);
            }
            else
            {

                    this.RawImage = e.Result;
               
            }
        }   
    }
}
