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
using System.Net;
using ShakrLabs.Mobile.App.Data.Providers.Base;
using System.Web;

namespace ShakrLabs.Mobile.App.Data.MA.Providers
{
    public class DataObjectResponse <T>
    {

        public T DataObject { get; private set; }

        public string ErrorMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public DataObjectResponse(T dataObject, string error)
        {
            DataObject = dataObject;
            ErrorMessage = error;
        }
        public DataObjectResponse( string error)
        {
            ErrorMessage = error;
        }
        public static DataObjectResponse<T> Create(T dataObject, string errorMessage)
        {
            var objectResponse = new DataObjectResponse<T>(dataObject, errorMessage);
            return objectResponse;
        }
        public static DataObjectResponse<T> Create(string errorMessage)
        {
            var objectResponse = new DataObjectResponse<T>(errorMessage);
            return objectResponse;
        }


     

    }
}