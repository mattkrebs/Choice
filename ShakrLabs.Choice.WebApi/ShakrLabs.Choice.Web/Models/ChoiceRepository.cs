using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShakrLabs.Choice.Data;

namespace ShakrLabs.Choice.Web.Models
{
    public class ChoiceRepository : IChoiceRepository
    {
        private ChoiceAppEntities db = new ChoiceAppEntities();

     
        public IEnumerable<ChoiceViewModel> GetAll()
        {
            List<ChoiceViewModel> model = new List<ChoiceViewModel>();
           
            var polls = db.Polls.Include(p => p.Category).Include(p => p.User).Include(p => p.PollItems);
            foreach (var item in polls)
            {
                List<PollItem> pollItems = item.PollItems.ToList();
                model.Add(new ChoiceViewModel()
                {
                    CategoryId = item.CategoryId,
                    ImageUrl1 = pollItems[0].ImageUrl,
                    ImageUrl2 = pollItems[1].ImageUrl,
                     MemberId = item.UserId,
                     PollId = item.PollId

                });
                
            }
            return model;
        }
        public string UploadImage(HttpPostedFileBase file)
        {
             CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               CloudConfigurationManager.GetSetting("StorageConnectionString"));

             // Create the blob client     
             CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

             // Retrieve a reference to a container 
             CloudBlobContainer container = blobClient.GetContainerReference("images");

             // Create the container if it doesn't already exist
             container.CreateIfNotExist();


             try
             {
                 var stream = file.InputStream;


                 CloudBlob blob = container.GetBlobReference(file.FileName);
                 blob.UploadFromStream(stream);
                 //Create Poll


                 return blob.Uri.OriginalString;
             }
             catch(Exception e)
             {
                 return "";
             }
                 
        }
        public ChoiceViewModel Get(Guid id)
        {
            throw new NotImplementedException();
        }
        private List<PollItem> CreatePollItems(ChoiceViewModel item)
        {
           List<PollItem> pItems = new List<PollItem>();
           PollItem pItem = new PollItem();
           pItem.PollItemId = Guid.NewGuid();
            pItem.Active = true;
            pItem.CreatedDate = DateTime.Now;
           //Upload image first and get url
            //TODO:
            if (!String.IsNullOrEmpty(item.ImageUrl1))
                pItem.ImageUrl = item.ImageUrl1;
            else
                pItem.ImageUrl = UploadImage(item.File1);

           PollItem pItem2 = new PollItem();
           pItem2.PollItemId = Guid.NewGuid();
           pItem2.Active = true;
           pItem2.CreatedDate = DateTime.Now;
           //Upload image first and get url
           if (!String.IsNullOrEmpty(item.ImageUrl2))
               pItem2.ImageUrl = item.ImageUrl2;
           else
               pItem2.ImageUrl = UploadImage(item.File2);


           pItems.Add(pItem);
           pItems.Add(pItem2);
           return pItems;
           
        }
        public ChoiceViewModel Add(ChoiceViewModel item)
        {
            if (item != null)
            {
                Poll p = new Poll()
                {
                    PollId = Guid.NewGuid(),
                    Active = true,
                    CategoryId = item.CategoryId,
                    UserId = new Guid("b6a63ad5-0e95-4585-80cf-ecd61438fc23"),
                    CreatedDate = DateTime.Now,
                    Sponsored = false,
                    PollItems = CreatePollItems(item),

                };

                db.Polls.Add(p);
                db.SaveChanges();
            }
            return item;
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Update(ChoiceViewModel item)
        {
            throw new NotImplementedException();
        }
    }
}