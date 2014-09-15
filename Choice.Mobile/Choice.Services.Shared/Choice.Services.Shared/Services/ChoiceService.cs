using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Choice.Services.Shared.Models;
using System.Security;
using Newtonsoft.Json;
using Choice.Services.Shared.ViewModels;
using System.IO;
using System.Net.Http.Headers;

namespace Choice.Services.Shared.Services
{
    public class ChoiceServices
    {
        private string _url = "https://choice.azurewebsites.net";
        public string BaseUri { get { return _url; } }

        public static List<ChoiceItem> Choices { get; set; }
        private static readonly ChoiceServices _instance = new ChoiceServices();
        public List<ExternalLoginViewModel> ExternalLogins { get; set; }
        public CookieContainer CookieContainer { get; set; }

        public bool LoggedIn { get; set; }

        public static ChoiceServices Instance
        {
            get
            {
                return _instance;
            }
        }

        public ChoiceServices() { }

        public async Task RegisterExternal(string email, string accessToken)
        {
             // where result is AuthResult containing the cookies obtained from WebBrowser's session
            try
            {
            
                using (var handler = new HttpClientHandler() { CookieContainer = ChoiceServices.Instance.CookieContainer })
                using (var client = new HttpClient(handler) { BaseAddress = new Uri(BaseUri) })
                {
                    var cookies = ChoiceServices.Instance.CookieContainer.GetCookies(new Uri(BaseUri));
                    foreach (Cookie cookie in ChoiceServices.Instance.CookieContainer.GetCookies(new Uri(BaseUri)))
                    {
                        Console.WriteLine("{0} - {1}",cookie.Name, cookie.Value);
                    }
                        
                    Console.WriteLine("Cookie Header:  " + handler.CookieContainer.GetCookieHeader(new Uri(BaseUri)));
                    // send request
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // setting Bearer Token obtained from Auth provider
                    client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", accessToken));

                    // calling /api/Account/UserInfo
                    var userInfoResponse = await client.GetAsync("api/Account/UserInfo");

                    var result = await userInfoResponse.Content.ReadAsStringAsync();
                   // models = JsonConvert.DeserializeObject<List<ExternalLoginViewModel>>(result);

                    var userInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfoViewModel>(result);

                    if (userInfo.HasRegistered == false)
                    {
                         RegisterExternalBindingModel model = new RegisterExternalBindingModel
                        {
                            Email = email
                        };

                        var registerExternalUrl = new Uri(string.Concat(BaseUri, @"/api/Account/RegisterExternal"));
                        var param = JsonConvert.SerializeObject(model);
                        HttpContent contentPost = new StringContent(param, Encoding.UTF8, "application/json");

                        var response = client.PostAsync(registerExternalUrl.ToString(), contentPost).Result;

                        // obtaining content
                        var responseContent = response.Content.ReadAsStringAsync().Result;

                        if (response != null && response.IsSuccessStatusCode)
                        {
                            Console.WriteLine("New user registered");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to register user", ex);
            } 
        }



        //public async Task RegisterExternal(string email, string accessToken)
        //{
        //    string uri = String.Format("{0}/api/Account/RegisterExternal", BaseUri);

        //    RegisterExternalBindingModel model = new RegisterExternalBindingModel
        //    {
        //        UserName = email
        //    };
        //    HttpWebRequest request = new HttpWebRequest(new Uri(uri));
        //    request.Headers.Add("Authorization", String.Format("Bearer {0}", accessToken));
        //    request.ContentType = "application/json";
        //    request.Accept = "application/json";
       
        //    request.Method = "POST";




        //    string postJson = "{ 'Email': 'krebs44@gmail.com'}"; //JsonConvert.SerializeObject(model);
        //    byte[] bytes = Encoding.UTF8.GetBytes(postJson);
        //    using (Stream requestStream = await request.GetRequestStreamAsync())
        //    {
        //        requestStream.Write(bytes, 0, bytes.Length);
        //    }

        //    try
        //    {
             
        //       WebResponse response = await request.GetResponseAsync();
        //       HttpWebResponse httpResponse = (HttpWebResponse)response;
        //       string result;

        //       using (Stream responseStream = httpResponse.GetResponseStream())
        //       {
        //           result = new StreamReader(responseStream).ReadToEnd();
        //           Console.WriteLine(result);
        //       }
        //    }
        //    catch (SecurityException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new InvalidOperationException("Unable to register user", ex);
        //    }
        //}
        public async Task<List<ChoiceItem>> GetChoices()
        {

            string uri = String.Format("{0}/api/Choices", BaseUri);          
           
            try
            {
                var httpClient = new HttpClient();
                var choices = await httpClient.GetStringAsync(uri);

                List<ChoiceItem> models = new List<ChoiceItem>();
                models = JsonConvert.DeserializeObject<List<ChoiceItem>>(choices);
                return models;
            }
            catch (SecurityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to get login providers", ex);
            }
        }

        //api/Account/ManageInfo?returnUrl=%2F&generateState=true
        public async Task<List<ExternalLoginViewModel>> GetExternalLoginProviders()
        {

            try
            {
                List<ExternalLoginViewModel> models = new List<ExternalLoginViewModel>();
                using (var handler = new HttpClientHandler() { CookieContainer = ChoiceServices.Instance.CookieContainer })
                using (var client = new HttpClient(handler) { BaseAddress = new Uri(BaseUri) })
                {
                    // send request
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage httpResponse = await client.GetAsync("api/Account/ExternalLogins?returnUrl=%2F&generateState=true");
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        //ChoiceServices.Instance.CookieContainer = handler.CookieContainer;
                        var result = await httpResponse.Content.ReadAsStringAsync();
                        models = JsonConvert.DeserializeObject<List<ExternalLoginViewModel>>(result);
                        
                    }
                }
                return models;
            }
            catch (SecurityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to get login providers", ex);
            }

            //string uri = String.Format("{0}/api/Account/ExternalLogins?returnUrl=%2F&generateState=true", BaseUri);
            //HttpWebRequest request = new HttpWebRequest(new Uri(uri));
            //request.Method = "GET";
            //try
            //{
            //    WebResponse response = await request.GetResponseAsync();
            //    HttpWebResponse httpResponse = (HttpWebResponse)response;
            //    string result;

            //    using (Stream responseStream = httpResponse.GetResponseStream())
            //    {
            //        result = new StreamReader(responseStream).ReadToEnd();
            //    }

            //    List<ExternalLoginViewModel> models = JsonConvert.DeserializeObject<List<ExternalLoginViewModel>>(result);
            //    return models;
            //}
            //catch (SecurityException)
            //{
            //    throw;
            //}
            //catch (Exception ex)
            //{
            //    throw new InvalidOperationException("Unable to get login providers", ex);
            //}
        }


        public string AccessToken { get; set; }
    }
}
