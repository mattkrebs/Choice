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

namespace Choice.Services.Shared.Services
{
    public class ChoiceServices
    {
        private string _url = "http://choice.azurewebsites.net";
        public string BaseUri { get { return _url; } }

        public static List<ChoiceItem> Choices { get; set; }
        private static readonly ChoiceServices _instance = new ChoiceServices();
        public List<ExternalLoginViewModel> ExternalLogins { get; set; }
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
            string uri = String.Format("{0}/api/Account/RegisterExternal", BaseUri);

            RegisterExternalBindingModel model = new RegisterExternalBindingModel
            {
                Email = email
            };
            HttpWebRequest request = new HttpWebRequest(new Uri(uri));
            request.Headers.Add("Authorization", String.Format("Bearer {0}", accessToken));
            request.ContentType = "application/json";
            request.Accept = "application/json";
       
            request.Method = "POST";

      


            string postJson = JsonConvert.SerializeObject(model);
            byte[] bytes = Encoding.UTF8.GetBytes(postJson);
            using (Stream requestStream = await request.GetRequestStreamAsync())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }

            try
            {
             
               WebResponse response = await request.GetResponseAsync();
               HttpWebResponse httpResponse = (HttpWebResponse)response;
               string result;

               using (Stream responseStream = httpResponse.GetResponseStream())
               {
                   result = new StreamReader(responseStream).ReadToEnd();
                   Console.WriteLine(result);
               }
            }
            catch (SecurityException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to register user", ex);
            }
        }
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
            string uri = String.Format("{0}/api/Account/ExternalLogins?returnUrl=%2F&generateState=true", BaseUri);
            HttpWebRequest request = new HttpWebRequest(new Uri(uri));
            request.Method = "GET";
            try
            {
                WebResponse response = await request.GetResponseAsync();
                HttpWebResponse httpResponse = (HttpWebResponse)response;
                string result;

                using (Stream responseStream = httpResponse.GetResponseStream())
                {
                    result = new StreamReader(responseStream).ReadToEnd();
                }

                List<ExternalLoginViewModel> models = JsonConvert.DeserializeObject<List<ExternalLoginViewModel>>(result);
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


        public string AccessToken { get; set; }
    }
}
