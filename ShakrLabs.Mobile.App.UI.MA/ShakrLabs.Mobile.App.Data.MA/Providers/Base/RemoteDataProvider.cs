using ShakrLabs.Mobile.App.Data.Providers.Response;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;


namespace ShakrLabs.Mobile.App.Data.Providers.Base
{
    /// <summary>
    /// Provides data from the public or member rest services for a specific type denoted by T
    /// </summary>
    /// <typeparam name="T">Type of response object to return.</typeparam>
    public class RemoteDataProvider<T> where T : class, new()
    {
        #region Variables & Properties

        private readonly LocalDataProvider<T> _localDataProvider;
        protected bool _authenticated = false;

        private string _serviceUri;
        private string _serviceUriBase;
        protected string _serviceUriSuffix;
        protected bool _useQueryString = false;

        public bool IsLoggedIn
        {
            get { return ServiceSessionManager.IsLoggedIn; }
            private set { ServiceSessionManager.IsLoggedIn = value; }
        }

        protected virtual string ServiceUriBase
        {
            get
            {
                if (_serviceUriBase == null)
                {
                    _serviceUriBase = _authenticated ? AppConfig.Current.MemberUriBase : AppConfig.Current.PublicUriBase;
                }
                return _serviceUriBase;
            }
        }

        private string ServiceUri
        {
            get
            {
                if (_serviceUri == null)
                {
                    _serviceUri = Path.Combine(ServiceUriBase, _serviceUriSuffix);
                    if (string.IsNullOrEmpty(_serviceUri))
                    {
                        throw new ApplicationException("Missing Service Uri for data provider");
                    }
                }
                return _serviceUri;
            }
        }

        #endregion

        protected RemoteDataProvider(string keyPrefix, bool memoryStorageEnabled, bool fileStorageEnabled)
        {
            _localDataProvider = new LocalDataProvider<T>(keyPrefix, memoryStorageEnabled, fileStorageEnabled);
#if DEBUG
            //disable security checks on server certificate (this is only for development
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
#endif
        }

        /// <summary>
        /// Logs the user in using the specified login URL.
        /// </summary>
        /// <param name="loginUri">The login URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public DataObjectResponse<bool> Login(string loginUri, string userName, string password)
        {
#if DEBUG

            if (string.IsNullOrEmpty(AppConfig.Current.LoginUri)) // testing against test service
            {
                Console.WriteLine("simulating login");
                Thread.Sleep(500);
                IsLoggedIn = true;
                return DataObjectResponse<bool>.Create(true, DataObjectSource.Local);
            }
#endif


            IsLoggedIn = false;
            var request = (HttpWebRequest) WebRequest.Create(loginUri);

            request.Method = "GET";
            request.Referer = loginUri;

            //save off information for later use
            ServiceSessionManager.Referrer = loginUri;
            ServiceSessionManager.MemberCookies = new CookieContainer();

            request.CookieContainer = ServiceSessionManager.MemberCookies;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Credentials = new NetworkCredential(userName, password);
            HttpWebResponse httpWebResponse;
            try
            {
                using (httpWebResponse = (HttpWebResponse) (request.GetResponse()))
                {
                    if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                    {
                        Console.WriteLine("Login Succeeded");
                        IsLoggedIn = true;
						ServiceSessionManager.MemberCookies.Add(httpWebResponse.Cookies);
                        return DataObjectResponse<bool>.Create(DataAccessError.CreateWithStatusCode(httpWebResponse.StatusCode), DataObjectSource.Remote);
                    }

                    Console.WriteLine("Login Failed");
                    return DataObjectResponse<bool>.Create(DataAccessError.CreateWithStatusCode(httpWebResponse.StatusCode), DataObjectSource.Remote);
                }
            }
            catch (WebException webException)
            {
                if (webException.Response != null)
                {
                    httpWebResponse = webException.Response as HttpWebResponse;
                    if (httpWebResponse != null)
                    {
                        if (httpWebResponse.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            return DataObjectResponse<bool>.Create(DataAccessError.CreateWithStatusCode(HttpStatusCode.Unauthorized, webException), DataObjectSource.Remote);
                        }
                    }
                }

                if (webException.Status == WebExceptionStatus.Timeout)
                {
                    return DataObjectResponse<bool>.Create(DataAccessError.CreateWithStatusCode(HttpStatusCode.RequestTimeout, webException), DataObjectSource.Remote);
                }

                if (webException.Status == WebExceptionStatus.Timeout)
                {
                    return DataObjectResponse<bool>.Create(DataAccessError.CreateWithStatusCode(HttpStatusCode.RequestTimeout, webException), DataObjectSource.Remote);
                }

                return DataObjectResponse<bool>.Create(DataAccessError.Create(webException), DataObjectSource.Remote);
            }
        }

        public void Logout()
        {
            IsLoggedIn = false;
            ServiceSessionManager.MemberCookies = null;
        }

        /// <summary>
        /// Executes the service method.
        /// </summary>
        /// <param name="uri">The URL.</param>
        /// <param name="verb">The verb.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        private DataObjectResponse<T> RequestRemoteData(string uri, HttpVerb verb, ParameterList parameters)
        {
			DataObjectResponse<T> dataDataObjectResponse;

			try
			{
	            HttpWebRequest request;
	           //Console.WriteLine("Get Object from Remote Service: " + uri);
	            const string contentType = "application/x-www-form-urlencoded";
	            const int timeout = 30000;

	            if (verb == HttpVerb.GET)
	            {
	                string queryString = GetQueryString(parameters);
                    Console.WriteLine("GET URL: " + uri + queryString);
	                request = (HttpWebRequest) WebRequest.Create(uri + queryString);
	                request.Timeout = timeout;
	                request.ContentType = contentType;
	                request.Method = "GET";
	                request.CookieContainer = _authenticated ?  ServiceSessionManager.MemberCookies : ServiceSessionManager.PublicCookies;
	            }
	            else // POST
	            {
	                request = (HttpWebRequest) WebRequest.Create(uri);
                    Console.WriteLine("POST URL: " + uri);

	                request.Timeout = timeout;
	                request.ContentType = contentType;
	                request.Method = "POST";
	                request.CookieContainer = _authenticated ?  ServiceSessionManager.MemberCookies : ServiceSessionManager.PublicCookies;

	                var postString = GetPostString(parameters);
#if DEBUG
                    Console.WriteLine("POST Parameters: " + postString);
#endif
	                byte[] postBytes = string.IsNullOrEmpty(postString) ? new byte[] {} : Encoding.ASCII.GetBytes(postString);
	                request.ContentLength = postBytes.Length;

	                Stream postStream = request.GetRequestStream();
	                postStream.Write(postBytes, 0, postBytes.Length);
	                postStream.Close();
	            }
	            request.AllowAutoRedirect = false;


				using (var response = (HttpWebResponse) (request.GetResponse()))
				{
                    if(_authenticated)
                    {
                        ServiceSessionManager.MemberCookies.Add(response.Cookies);
                    }
                    else
                    {
                        ServiceSessionManager.PublicCookies.Add(response.Cookies);
                    }

					var dataAccessError = DataAccessError.CreateWithStatusCode(response.StatusCode);
					if (dataAccessError != null)
					{
						return DataObjectResponse<T>.Create(dataAccessError, DataObjectSource.Remote);
					}

					using (var stream = response.GetResponseStream())
					{
						using (var reader = new StreamReader(stream) as TextReader)
						{
							//verify media type
							string serializedObjectString = reader.ReadToEnd();
						    if (string.IsNullOrWhiteSpace(serializedObjectString))
							{
                                dataDataObjectResponse = DataObjectResponse<T>.Create(DataAccessError.Create("No data returned in HTTP response"), DataObjectSource.Remote);
							}
                            else
							{
								try
								{
								    //ISerializer<T> serializer = SerializerFactory.Create<T>(AppConfig.Current.ProviderSerializationFormat);
                                    //T dataObject = serializer.DeserializeObject(serializedObjectString);

                                    var dataObject = Serialization.DeserializeObject<T>(serializedObjectString);

								    dataDataObjectResponse = DataObjectResponse<T>.Create(dataObject, DataObjectSource.Remote);
								}
								catch (Exception e)
								{
									//Console.WriteLine(string.Format("Encountered exception {0} while deserializing httpWebResponse: stack trace:{1}", e.Message, e.StackTrace));
                                    Console.WriteLine("Encountered exception while deserializing httpWebResponse:" + e.Message);
                                    dataDataObjectResponse = DataObjectResponse<T>.Create(DataAccessError.Create(e), DataObjectSource.Remote);
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
                    dataDataObjectResponse = DataObjectResponse<T>.Create(DataAccessError.CreateWithStatusCode(HttpStatusCode.RequestTimeout, webException), DataObjectSource.Remote);
				}
				else
				{
					dataDataObjectResponse = DataObjectResponse<T>.Create(DataAccessError.Create(webException), DataObjectSource.Remote);
				}
			}
			return dataDataObjectResponse;
		}

        #region Methods

        #region Set Methods

        public void SetObject(T objectToSet)
        {
            _localDataProvider.SetObject(objectToSet);
            // enable POST
        }

        public void SetObject(string key, T objectToSet)
        {
            _localDataProvider.SetObject(key, objectToSet);
            // enable POST
        }

        #endregion

        #region Get Methods

        public bool CheckObjectInMemory(ParameterList parameters)
        {
            // Local Key
            string dataKey = GetLocalDataKey(parameters);

            // Memory
            return _localDataProvider.CheckObjectInMemory(dataKey);
        }

        protected DataObjectResponse<T> GetObjectResponse(ParameterList parameters = null)
        {
            // Local Key
            string dataKey = GetLocalDataKey(parameters);


            // Memory and File
            T deserializedObject = _localDataProvider.GetObject(dataKey);
            if (deserializedObject != null)
            {
				return DataObjectResponse<T>.Create(deserializedObject, DataObjectSource.Remote);
            }

            // Data Service
            var useGet = (_useQueryString || parameters == null || parameters.Count == 0);
            var httpVerb = useGet ? HttpVerb.GET : HttpVerb.POST;
            DataObjectResponse<T> dataObjectResponse = RequestRemoteData(ServiceUri, httpVerb, parameters);

            if (!dataObjectResponse.HasError && dataObjectResponse.DataObject != null)
            {
                _localDataProvider.SetObject(dataKey, dataObjectResponse.DataObject);
            }
        
			return dataObjectResponse;
        }

        #endregion

        public void Clear()
        {
            _localDataProvider.Clear();
        }

        private static string GetPostString(ParameterList parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return String.Empty;
            }

            string queryString = null;
            foreach (Parameter item in parameters)
            {
                if (!String.IsNullOrWhiteSpace(item.Value))
                {
                    queryString = String.Format("{0}{1}={2}&", queryString, item.Name, HttpUtility.UrlEncode(item.Value));
                }
            }

            if (queryString != null) queryString = queryString.TrimEnd('&');
            return queryString;
        }

        private static string GetQueryString(ParameterList parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return String.Empty;
            }

            string queryString = null;
            foreach (Parameter item in parameters)
            {
                if (!String.IsNullOrWhiteSpace(item.Value))
                {
                    queryString = String.Format("{0}{1}={2}&", queryString, item.Name, HttpUtility.UrlEncode(item.Value));
                }
            }

            if (queryString != null) queryString = queryString.TrimEnd('&');
            return "?" + queryString;
        }

        private static string GetLocalDataKey(ParameterList parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return null;
            }

            string memoryKey = null;
            foreach (Parameter item in parameters)
            {
                if (!String.IsNullOrWhiteSpace(item.Value))
                {
                    memoryKey = String.Format("{0}{1}:{2}-", memoryKey, item.Name, item.Value);
                }
            }

            return "_" + memoryKey;
        }

        #endregion
    }
}