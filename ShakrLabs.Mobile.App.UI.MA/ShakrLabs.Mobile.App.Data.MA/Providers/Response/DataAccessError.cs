using System;
using System.Net;

namespace  ShakrLabs.Mobile.App.Data.Providers.Response
{
    public class DataAccessError
    {
        #region Fields
        #endregion Fields


        #region Properties

        public Exception ErrorException { get; private set; }

        public bool IsNoConnectivity
        {
            get { return IsNoConnectivityStatusCode(StatusCode); }
        }

        public bool Unauthorized { get; private set; }


        public HttpStatusCode StatusCode { get; private set; }

        private string _message;
        public string Message
        {
            get
            {
                const string defaultMessage = "The system is not responding, please try again later";
                string testingMessage = "";
#if DEBUG
                if (string.IsNullOrEmpty(_message) && ErrorException != null)
                {
                    testingMessage = "Exception: " + ErrorException.Message;
                }
                else
                {
                    testingMessage = _message;
                }
                testingMessage = " \r\n(Testing Only) " + testingMessage;
#endif
                return defaultMessage + testingMessage;
            }

            private set
            {
                _message = value;
            }
        }

        #endregion Properties


        #region Constructors & Initialization

        public static DataAccessError Create(string message)
        {
            var dataAccessError = new DataAccessError();
            dataAccessError.Message = message;
            return dataAccessError;
        }

        public static DataAccessError Create(Exception exception)
        {
            LogException(exception);

            var dataAccessError = new DataAccessError();
            dataAccessError.ErrorException = exception;
            return dataAccessError;
        }

        public static DataAccessError CreateWithStatusCode(HttpStatusCode statusCode, Exception exception = null)
        {
            var statusCodeInt = (int)statusCode;
            if (statusCodeInt >= 200 && statusCodeInt <= 202)
            {
                return null;
            }

            Console.WriteLine("Data Access Error - HTTP Status Code: '" + statusCode + "' (" + statusCodeInt + ")");

            LogException(exception);
            var dataAccessError = new DataAccessError { ErrorException = exception, StatusCode = statusCode };

            switch (statusCode)
            {
                case HttpStatusCode.Unauthorized:
                    dataAccessError.Unauthorized = true;
                    dataAccessError.Message = "Authentication failed.";
                    break;
                case HttpStatusCode.RequestTimeout:
                    dataAccessError.Message = "Timed out waiting for response from server.";
                    break;
                case HttpStatusCode.NoContent:
                    dataAccessError.Message = "Information not found on server.";
                    break;
                default:
                    if (IsNoConnectivityStatusCode(statusCode))
                    {
                        dataAccessError.Message = "Unable to connect to the server.";
                    }
                    else
                    {
                        dataAccessError.Message = "An error occurred accessing the information you requested: HTTP Status Code: '" + statusCode + "' (" + statusCodeInt + ")";
                    }
                    break;
            }

            return dataAccessError;
        }
        #endregion


        #region Methods

        private static bool IsNoConnectivityStatusCode(HttpStatusCode httpStatusCode)
        {
            return (httpStatusCode == HttpStatusCode.BadGateway);
        }

        private static void LogException(Exception exception)
        {
            if (exception != null)
            {
                Console.WriteLine("DataAccess Exception: '{0}', Stack trace: {1}", exception.Message, exception.StackTrace);
            }
        }

        #endregion Methods
    }
}