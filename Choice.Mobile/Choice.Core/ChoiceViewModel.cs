using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;
using Choice.Core.Models;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.IO;

namespace Choice.Core
{
    public class ChoiceViewModel : ViewModel<ChoiceItem>
    {
        MobileServiceClient db = new MobileServiceClient("https://choice.azure-mobile.net/", "VQiwdHPOJmRlpXFtwzUCOvOHboUspI17");
        
        public ChoiceItem Choice { get; set; }

        public List<ChoiceItem> Choices { get; set; }


        public Stream ImageStream1 { get; set; }
        public Stream ImageStream2 { get; set; }



        string[] urls = new string[] { "http://www.lolcats.com/images/u/12/24/lolcatsdotcompromdate.jpg",
            "http://www.lolcats.com/images/u/11/23/lolcatsdotcomuu378xml5m6xkh1c.jpg", 
            "http://www.lolcats.com/images/u/08/35/lolcatsdotcomaxdjl1t6rivbjr5u.jpg", 
            "http://www.lolcats.com/images/u/12/24/lolcatsdotcomseriously.jpg", 
            "http://www.lolcats.com/images/u/08/32/lolcatsdotcombkf8azsotkiwu8z2.jpg",
            "http://www.lolcats.com/images/u/09/03/lolcatsdotcomqicuw9j0uqt8a23t.jpg", 
            "http://www.lolcats.com/images/u/12/24/lolcatsdotcompromdate.jpg", 
            "http://3.bp.blogspot.com/-iWZ2WzwPr7E/TciqLegMuHI/AAAAAAAAAD0/bAuHtfDO-5w/s1600/lolcats.jpg" };


        public void GetImage(Action<byte[]> success)
        {
            Thread.Sleep(200);
            Random random = new Random();
            int randomNumber = random.Next(0, 8);

            GetImage(urls[randomNumber], success);
        }


        public void GetImage(string path, Action<byte[]> success)
        {
            var webClient = new WebClient();
            webClient.DownloadDataCompleted += (sender, e) =>
            {
                success(e.Result);                
                // do something with downloaded string, do UI interaction on main thread
            };

            webClient.DownloadDataAsync(new Uri(path));
        }

        public void MakeChoiceAsync(Guid guid, int image)
        {
            
        }

        public void GetChoicesAsync(Action<List<ChoiceItem>> success)
        {
           // List<ChoiceItem> items = new List<ChoiceItem>();
            db.GetTable<ChoiceItem>().ToListAsync().ContinueWith(t => 
            {
                if (!t.IsFaulted)
                {
                    if (ShowAlert != null)
                        ShowAlert("Some Error", "It occured here");                   
                }
                else
                {                   
                        success(t.Result); 
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());            
        }

        public void SaveChoiceAsync(ChoiceItem item, Action success)
        {
            try
            {
                if (string.IsNullOrEmpty(item.Id))
                    db.GetTable<ChoiceItem>().InsertAsync(item).ContinueWith(t => {
                        if (!t.IsFaulted)
                        {
                            if (ShowAlert != null)
                                ShowAlert("Some Error Saving", "Saving Choice Failed");

                        }
                        else
                        {
                            success();
                        }
                    },TaskScheduler.FromCurrentSynchronizationContext());
                else
                    db.GetTable<ChoiceItem>().UpdateAsync(item).ContinueWith(t =>
                    {
                        if (!t.IsFaulted)
                        {
                            if (ShowAlert != null)
                                ShowAlert("Some Error Saving", "Saving Choice Failed");

                        }
                        else
                        {
                            success();
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            catch (Exception ex)
            {
                ShowAlert("Error Saving Choice", ex.Message);
            }
        }
    }
}
