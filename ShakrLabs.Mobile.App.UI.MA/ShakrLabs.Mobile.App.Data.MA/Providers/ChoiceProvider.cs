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
using System.Threading.Tasks;
using Java.Lang;
using ShakrLabs.Mobile.App.Data.MA.ServiceModel;
using ShakrLabs.Mobile.App.Data.ViewModels;
using ShakrLabs.Mobile.App.Data.MA.Providers;

namespace ShakrLabs.Mobile.App.Data.Providers
{
    public class ChoiceProvider : RemoteDataProvider<ChoiceViewModel>
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
            : base(AppConfig.Current.PublicUriBase + "GetBatch/")
        {
         
        }
        

        public DataObjectResponse<ChoiceViewModel> GetRandomChoices(string token)
        {
            var parameters = new Dictionary<string,string>()
            {
                {"token",token}
            };
            var serviceRet = GetObjectResponse(parameters);
            return serviceRet;
        }

        public ChoiceViewModel GetChoices(Dictionary<string, string> parameters)
        {
            
            Poll polls = new Poll()
            {
               
                Images = new List<Image>() {pollImage(), pollImage()},
                Cat = 1,
                TotRats = 33,
                PollId = Guid.NewGuid().ToString()
            };

            ChoiceViewModel c = new ChoiceViewModel()
            {
                BatchId = "asdf",
                Polls = new List<Poll>() { polls }
            };
             
           
            return c;

        }

        string[] urls = new string[] { "http://www.lolcats.com/images/u/12/24/lolcatsdotcompromdate.jpg",
            "http://www.lolcats.com/images/u/11/23/lolcatsdotcomuu378xml5m6xkh1c.jpg", 
            "http://www.lolcats.com/images/u/08/35/lolcatsdotcomaxdjl1t6rivbjr5u.jpg", 
            "http://www.lolcats.com/images/u/12/24/lolcatsdotcomseriously.jpg", 
            "http://www.lolcats.com/images/u/08/32/lolcatsdotcombkf8azsotkiwu8z2.jpg",
            "http://www.lolcats.com/images/u/09/03/lolcatsdotcomqicuw9j0uqt8a23t.jpg", 
            "http://www.lolcats.com/images/u/12/24/lolcatsdotcompromdate.jpg", 
            "http://3.bp.blogspot.com/-iWZ2WzwPr7E/TciqLegMuHI/AAAAAAAAAD0/bAuHtfDO-5w/s1600/lolcats.jpg" };
       

        public Image pollImage()
        {
            var i = new Image();
            Thread.Sleep(200);
            Random random = new Random();
            int randomNumber = random.Next(0, 8);
            i.Id = Guid.NewGuid().ToString();
            i.Rating = 40;
            i.Url = urls[randomNumber];
            
            return i;

        }


      
    }
}