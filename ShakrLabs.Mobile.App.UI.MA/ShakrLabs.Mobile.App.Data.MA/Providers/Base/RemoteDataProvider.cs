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
using ShakrLabs.Mobile.App.Data.Providers.Base;
using System.Net;
using System.IO;
using System.Web;

namespace ShakrLabs.Mobile.App.Data.MA.Providers
{
    public class RemoteDataProvider<T>
    {      

        public string ServiceURI { get; set; }
        public Dictionary<string, string> Parameters { get; set; }

        public RemoteDataProvider(string serviceUri)
        {
            ServiceURI = serviceUri;          
        }
        protected DataObjectResponse<T> GetObjectResponse(Dictionary<string,string> parameters = null)
        {
           // Data Service
           DataObjectResponse<T> dataObjectResponse = RequestRemoteData(ServiceURI, HttpVerb.GET, parameters);
            
           return dataObjectResponse;
        }


        /// Executes the service method.
        /// </summary>
        /// <param name="uri">The URL.</param>
        /// <param name="verb">The verb.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        private DataObjectResponse<T> RequestRemoteData(string uri, HttpVerb verb, Dictionary<string, string> parameters)
        {
            DataObjectResponse<T> dataDataObjectResponse;

            try
            {
                HttpWebRequest request;
                //Console.WriteLine("Get Object from Remote Service: " + uri);
                const string contentType = "application/json";
                const int timeout = 30000;

                if (verb == HttpVerb.GET)
                {
                    string queryString = GetQueryString(parameters);
                    Console.WriteLine("GET URL: " + uri + queryString);
                    request = (HttpWebRequest)WebRequest.Create(uri + queryString);
                    request.Timeout = timeout;
                    request.ContentType = contentType;
                    request.Method = "GET";
                    //request.CookieContainer = _authenticated ? ServiceSessionManager.MemberCookies : ServiceSessionManager.PublicCookies;
                }
                else // POST
                {
                    request = (HttpWebRequest)WebRequest.Create(uri);
                    Console.WriteLine("POST URL: " + uri);

                    request.Timeout = timeout;
                    request.ContentType = contentType;
                    request.Method = "POST";
                    // request.CookieContainer = _authenticated ? ServiceSessionManager.MemberCookies : ServiceSessionManager.PublicCookies;

                    var postString = GetPostString(parameters);
#if DEBUG
                    Console.WriteLine("POST Parameters: " + postString);
#endif
                    byte[] postBytes = string.IsNullOrEmpty(postString) ? new byte[] { } : Encoding.ASCII.GetBytes(postString);
                    request.ContentLength = postBytes.Length;

                    Stream postStream = request.GetRequestStream();
                    postStream.Write(postBytes, 0, postBytes.Length);
                    postStream.Close();
                }
                request.AllowAutoRedirect = false;


                using (var response = (HttpWebResponse)(request.GetResponse()))
                {
                    //if (_authenticated)
                    //{
                    //    ServiceSessionManager.MemberCookies.Add(response.Cookies);
                    //}
                    //else
                    //{
                    //    ServiceSessionManager.PublicCookies.Add(response.Cookies);
                    //}

                    var dataAccessError = CreateMessageWithStatusCode(response.StatusCode);
                    if (!String.IsNullOrEmpty(dataAccessError))
                    {
                        return DataObjectResponse<T>.Create(dataAccessError);
                    }

                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream) as TextReader)
                        {
                            //verify media type
                            string serializedObjectString = reader.ReadToEnd();
                            if (string.IsNullOrWhiteSpace(serializedObjectString))
                            {
                                dataDataObjectResponse = DataObjectResponse<T>.Create("No data returned in HTTP response");
                            }
                            else
                            {
                                try
                                {
                                    //ISerializer<T> serializer = SerializerFactory.Create<T>(AppConfig.Current.ProviderSerializationFormat);
                                    //T dataObject = serializer.DeserializeObject(serializedObjectString);

                                    var dataObject = Serialization.DeserializeObject<T>(serializedObjectString);

                                    dataDataObjectResponse = DataObjectResponse<T>.Create(dataObject, "");
                                }
                                catch (Exception e)
                                {
                                    //Console.WriteLine(string.Format("Encountered exception {0} while deserializing httpWebResponse: stack trace:{1}", e.Message, e.StackTrace));
                                    Console.WriteLine("Encountered exception while deserializing httpWebResponse:" + e.Message);
                                    dataDataObjectResponse = DataObjectResponse<T>.Create(e.Message);
                                }
                            }
                        }
                    }
                }
                return dataDataObjectResponse;
            }
            catch (WebException webException)
            {
                Console.WriteLine("WebException:" + webException.Message);
                if (webException.Status == WebExceptionStatus.Timeout)
                {
                    dataDataObjectResponse = DataObjectResponse<T>.Create(CreateMessageWithStatusCode(HttpStatusCode.RequestTimeout, webException));
                }
                else
                {
                    dataDataObjectResponse = DataObjectResponse<T>.Create(webException.Message);
                }
            }
            return dataDataObjectResponse;
        }


        private static string GetQueryString(Dictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return String.Empty;
            }

            string queryString = null;
            foreach (KeyValuePair<string, string> item in parameters)
            {
                if (!String.IsNullOrWhiteSpace(item.Value))
                {
                    queryString = String.Format("{0}{1}={2}&", queryString, item.Key, HttpUtility.UrlEncode(item.Value));
                }
            }

            if (queryString != null) queryString = queryString.TrimEnd('&');
            return "?" + queryString;
        }

        private static string GetPostString(Dictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return String.Empty;
            }

            string queryString = null;
            foreach (KeyValuePair<string, string> item in parameters)
            {
                if (!String.IsNullOrWhiteSpace(item.Value))
                {
                    queryString = String.Format("{0}{1}={2}&", queryString, item.Key, HttpUtility.UrlEncode(item.Value));
                }
            }

            if (queryString != null) queryString = queryString.TrimEnd('&');
            return queryString;
        }

        public static string CreateMessageWithStatusCode(HttpStatusCode statusCode, Exception exception = null)
        {
            string ErrorMessage;
            var statusCodeInt = (int)statusCode;
            if (statusCodeInt >= 200 && statusCodeInt <= 202)
            {
                return null;
            }

            Console.WriteLine("Data Access Error - HTTP Status Code: '" + statusCode + "' (" + statusCodeInt + ")");

            //LogException(exception);
           // var dataAccessError = new DataAccessError { ErrorException = exception, StatusCode = statusCode };

            switch (statusCode)
            {
                case HttpStatusCode.Unauthorized:
                    ErrorMessage = "Authentication failed.";
                    break;
                case HttpStatusCode.RequestTimeout:
                    ErrorMessage = "Timed out waiting for response from server.";
                    break;
                case HttpStatusCode.NoContent:
                    ErrorMessage = "Information not found on server.";
                    break;
                default:
                    if (statusCode == HttpStatusCode.BadGateway)
                    {
                        ErrorMessage = "Unable to connect to the server.";
                    }
                    else
                    {
                        ErrorMessage = "An error occurred accessing the information you requested: HTTP Status Code: '" + statusCode + "' (" + statusCodeInt + ")";
                    }
                    break;
            }

            return ErrorMessage;
        }
    }
}