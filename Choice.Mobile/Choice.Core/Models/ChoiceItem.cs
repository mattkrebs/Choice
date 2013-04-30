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

namespace Choice.Core.Models
{
    public class ChoiceItem
    {
        public String Id { get; set; }     
        public String Category { get; set; }
        public String ImagePath1 { get; set; }
        public String ImagePath2 { get; set; }
        public String UserId { get; set; }
        
    }
}