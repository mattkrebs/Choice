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
    public class Category
    {
         [PrimaryKey]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

    }
}