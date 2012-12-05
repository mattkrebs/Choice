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
using SQLite;

namespace ShakrLabs.Mobile.App.Data.ViewModels
{
    
    public class Image
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public double Rating { get; set; }
    }

    public class Poll
    {
        public string PollId { get; set; }
        public int Cat { get; set; }
        public List<Image> Images { get; set; } 
        public int TotRats { get; set; }
        public int I1Rats { get; set; }
        public int I2Rats { get; set; }
    }

    public class ChoiceViewModel
    {
        public List<Poll> Polls { get; set; }
        public string BatchId { get; set; }
    }

    
}