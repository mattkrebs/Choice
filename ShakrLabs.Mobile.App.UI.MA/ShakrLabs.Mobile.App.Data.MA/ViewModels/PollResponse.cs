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

namespace ShakrLabs.Mobile.App.Data.Models
{
    public class PollResponse
    {
        public List<Poll> Polls { get; set; }
    }
}