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
using ShakrLabs.Mobile.App.Data.ViewModels;

namespace ShakrLabs.Mobile.App.Data.MA.ServiceModel
{
    public class ChoiceResponse
    {
        public List<ChoiceViewModel> Choices { get; set; }
    }
}