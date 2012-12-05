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
using ShakrLabs.Mobile.App.Data.Providers;
using System.Net;
using Android.Util;
using System.Threading.Tasks;
using ShakrLabs.Mobile.App.Data.ViewModels;
using ShakrLabs.Mobile.App.Data.MA.ServiceModel;
using ShakrLabs.Mobile.App.Data.MA.Providers;

namespace ShakrLabs.Mobile.App.Shared.Presenter
{
    public class ChoicePresenter
    {
        protected readonly ISharedApp _sharedApp;

        public ChoicePresenter()
            : this(SharedApp.Current)
        {
        }

        public ChoicePresenter(ISharedApp sharedApp)
        {
            _sharedApp = sharedApp;
        }
        private static ChoicePresenter _presenter;
        public static ChoicePresenter Current
        {
            get
            {
                if (_presenter == null)
                {
                    _presenter = new ChoicePresenter();
                }
                return _presenter;
            }
        }


        public ChoiceViewModel NewChoice { get; set; }


        private DataObjectResponse<ChoiceViewModel> GetRandomChoices(string token)
        {
          
           DataObjectResponse<ChoiceViewModel> response = ChoiceProvider.Current.GetRandomChoices(token);
           return response;

        }


        public void GetRandomChoicesAsync(Action<ChoiceViewModel> successMethod, string token)
        {
            _sharedApp.GetDataAsync(() => GetRandomChoices(token), successMethod);           
        }


        public ChoiceViewModel GetChoice()
        {
           return ChoiceProvider.Current.GetChoices(null);
        }
    }
}