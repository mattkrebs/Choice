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
    public class Poll
    {
        public System.Guid PollId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool Active { get; set; }

        public List<PollItem> PollItems { get; set; }




    }
}