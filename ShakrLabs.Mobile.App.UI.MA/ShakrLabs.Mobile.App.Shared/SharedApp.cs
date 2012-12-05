using ShakrLabs.Mobile.App.Data;
using ShakrLabs.Mobile.App.Data.MA.Providers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShakrLabs.Mobile.App.Shared
{
    public class SharedApp : ISharedApp
    {
        public Action<bool> ShowActivityIndictor;
        public Action<string> ShowErrorMessage;
        private bool _taskRunning;
        private static SharedApp _sharedApp;
        public static SharedApp Current
        {
            get { return _sharedApp ?? (_sharedApp = new SharedApp()); }
        }

        public void OnShowErrorMessage(string message)
        {
            if (ShowErrorMessage != null)
            {
                ShowErrorMessage(message);
            }
        }

        public void OnShowActivityIndicator()
        {
            if (ShowActivityIndictor != null)
            {
                Console.WriteLine("OnShowActivityIndicator");
                ShowActivityIndictor(true);
            }
        }

        public void OnHideActivityIndicator()
        {
            if (ShowActivityIndictor != null)
            {
                Console.WriteLine("OnHideActivityIndicator");
                ShowActivityIndictor(false);
            }
        }

        public void GetDataAsync<T>(Func<DataObjectResponse<T>> dataAccessFunction, Action<T> successAction, Action<string> failureAction = null)
        {
			_taskRunning = false;

            // Show the loading overlay on the UI thread
			Task.Factory.StartNew
			(
				() =>
				{
					Thread.Sleep(200);
				}
			).ContinueWith
			(
				t =>
				{
					if (_taskRunning)
					{
						OnShowActivityIndicator();
					}
				},
				TaskScheduler.FromCurrentSynchronizationContext()
			);

            _taskRunning = true;

            // Create a new thread to do some long running work using StartNew
            Task<DataObjectResponse<T>>.Factory.StartNew
            (
                () =>
                {
                    Console.WriteLine("starting background task.");
                    try
                    {
                        return dataAccessFunction();
                    }
                    catch (Exception e)
                    {
                        return DataObjectResponse<T>.Create(e.Message);
                    }
                }
                // ContinueWith accommodates an action that runs after the previous thread completes.
                // By using TaskScheduler.FromCurrentSyncrhonizationContext, this task now runs on the original calling thread, 
                // in this case the UI thread, so that any UI updates are safe.
            ).ContinueWith
            (
                t =>
                    {
                        _taskRunning = false;

                        var response = t.Result;
                        if (!String.IsNullOrEmpty(response.ErrorMessage))
                        {
                            OnHideActivityIndicator();
                            if (failureAction == null)
                            {
                                OnShowErrorMessage(response.ErrorMessage);
                            }
                            else
                            {
                                failureAction(response.ErrorMessage);
                            }
                        }
                        else
                        {
                            OnHideActivityIndicator();
                            successAction(response.DataObject);
                        }
                        Console.WriteLine("Hid activity indicator from the UI thread.");
                },
                TaskScheduler.FromCurrentSynchronizationContext()
            );
        }

        public static void SetAppAssetStringDelegate(Func<string, string> appAssetStringDelegate)
        {
            AppConfig.Current.SetAppAssetStringDelegate(appAssetStringDelegate);
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public static void Reset()
        {
            AppConfig.Current.DeleteAllDataFiles();

            ClearUserMemory();
        }

        public static void ClearUserMemory()
        {
       
        }
    }
}