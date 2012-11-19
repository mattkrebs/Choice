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
using System.Threading.Tasks;
using Java.Lang;
using ShakrLabs.Mobile.App.Data.Providers.Base;
using ShakrLabs.Mobile.App.Data.Providers.Response;
using ShakrLabs.Mobile.App.Data.MA.ServiceModel;
using ShakrLabs.Mobile.App.Data.ViewModels;

namespace ShakrLabs.Mobile.App.Data.Providers
{
    public class ChoiceProvider : RemoteDataProvider<ChoiceResponse>
    {
        int previous;
        private static ChoiceProvider _choiceProvider;
        public static ChoiceProvider Current
        {
            get
            {
                if (_choiceProvider == null)
                {
                    _choiceProvider = new ChoiceProvider();
                }
                return _choiceProvider;
            }
        }

        private ChoiceProvider()
            : base("ChoiceProvider", true, true)
        {
            _serviceUriSuffix = "Choice/";
            _authenticated = true;
        }


        public DataObjectResponse<ChoiceResponse> GetRandomChoiceViewModel(string userId)
        {
            var parameters = new ParameterList
            {
                {"userid",userId}
            };
            var serviceRet = GetObjectResponse(parameters);
            return serviceRet;
        }

        public ChoiceViewModel GetChoice(Dictionary<string, string> parameters)
        {
            ChoiceViewModel polls = new ChoiceViewModel()
            {
                CategoryId = 1,
                ImageUrl1 = pollImage(),
                ImageUrl2 = pollImage(),
                MemberId = Guid.NewGuid(),
                PollId = Guid.NewGuid()
            };
            

             
           
            return polls;

        }

        string[] urls = new string[] { "http://www.lolcats.com/images/u/12/24/lolcatsdotcompromdate.jpg",
            "http://www.lolcats.com/images/u/11/23/lolcatsdotcomuu378xml5m6xkh1c.jpg", 
            "http://www.lolcats.com/images/u/08/35/lolcatsdotcomaxdjl1t6rivbjr5u.jpg", 
            "http://www.lolcats.com/images/u/12/24/lolcatsdotcomseriously.jpg", 
            "http://www.lolcats.com/images/u/08/32/lolcatsdotcombkf8azsotkiwu8z2.jpg",
            "http://www.lolcats.com/images/u/09/03/lolcatsdotcomqicuw9j0uqt8a23t.jpg", 
            "http://www.lolcats.com/images/u/12/24/lolcatsdotcompromdate.jpg", 
            "http://3.bp.blogspot.com/-iWZ2WzwPr7E/TciqLegMuHI/AAAAAAAAAD0/bAuHtfDO-5w/s1600/lolcats.jpg" };


        public string pollImage()
        {
            Thread.Sleep(200);
            Random random = new Random();
            int randomNumber = random.Next(0, 8);

            
            return urls[randomNumber];

        }


      
    }
}