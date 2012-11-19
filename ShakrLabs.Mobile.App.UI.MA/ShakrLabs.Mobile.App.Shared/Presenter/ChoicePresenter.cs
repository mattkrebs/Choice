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
using ShakrLabs.Mobile.App.Data.Models;
using ShakrLabs.Mobile.App.Data.Providers;
using System.Net;
using Android.Util;
using System.Threading.Tasks;
using ShakrLabs.Mobile.App.Data.Providers.Response;
using ShakrLabs.Mobile.App.Data.ViewModels;
using ShakrLabs.Mobile.App.Data.MA.ServiceModel;

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

        private DataObjectResponse<List<ChoiceViewModel>> GetRandomChoiceViewModel(string userId)
        {
            var ret = new ChoiceResponse();
            DataObjectResponse<ChoiceResponse> response = ChoiceProvider.Current.GetRandomChoiceViewModel(userId);
           if (response.HasError)
           {
               return DataObjectResponse<List<ChoiceViewModel>>.Create(response, null);
           }

           return DataObjectResponse<List<ChoiceViewModel>>.Create(response.DataObject.Choices, DataObjectSource.Remote);

        }


        public void GetRandomChoicesAsync(Action<List<ChoiceViewModel>> successMethod, string userId)
        {
            _sharedApp.GetDataAsync(() => GetRandomChoiceViewModel(userId), successMethod);           
        }




        public ChoiceViewModel GetChoice()
        {
           return ChoiceProvider.Current.GetChoice(null);
        }
    }
}