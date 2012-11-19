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

namespace ShakrLabs.Mobile.App.Data.ViewModels
{
    public class ChoiceViewModel
    {
        public Guid PollId { get; set; }
        public string ImageUrl1 { get; set; }
        public string ImageUrl2 { get; set; }
        public byte[] File1 { get; set; }
        public byte[] File2 { get; set; }
        public byte CategoryId { get; set; }
        public Guid MemberId { get; set; }
    }
}