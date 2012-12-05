
using ShakrLabs.Mobile.App.Data.MA.Providers;
using System;


namespace ShakrLabs.Mobile.App.Shared
{
    public interface ISharedApp
    {
        void OnShowErrorMessage(string message);
        void OnShowActivityIndicator();
        void OnHideActivityIndicator();
        void GetDataAsync<T>(Func<DataObjectResponse<T>> dataAccessFunction, Action<T> successAction, Action<string> failureAction = null);
    }
}