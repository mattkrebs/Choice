using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using ShakrLabs.Choice.Data;
using ShakrLabs.Choice.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShakrLabs.Choice.Web.Controllers
{
    public class ChoiceController : Controller
    {
        //
        // GET: /Choice/

        private ChoiceTestEntities db = new ChoiceTestEntities();


        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Choice/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Choice/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Choice/Create

        [HttpPost]
        public ActionResult Create(PollRequestModel model)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the blob client 
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container 
            CloudBlobContainer container = blobClient.GetContainerReference("images");

            // Create the container if it doesn't already exist
            container.CreateIfNotExist();
            Poll poll = new Poll()
            {
                PollId = Guid.NewGuid(),
                Active = true,
                CategoryId = model.Category,
                CreatedDate = DateTime.Now,
                UserId = 1
            };
            foreach (var file in model.Files)
            {
                try
                {
                    var stream = file.InputStream;
                   
                    
                    CloudBlob blob = container.GetBlobReference(file.FileName);
                    blob.UploadFromStream(stream);
                    //Create Poll
                   
                    poll.PollItems.Add(new PollItem(){
                        PollItemId = Guid.NewGuid(),
                        Active = true,
                        CreatedDate = DateTime.Now,
                        ImageUrl = blob.Uri.OriginalString,
                        UserId = 1
                    });

                   
                    
                    //blob.UploadFile(file);
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message }, "application/json");
                }
            }
            db.Polls.Add(poll);
            db.SaveChanges();
            return Json(new { success = true }, "text/html");

         
        }
      

        //
        // GET: /Choice/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Choice/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Choice/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Choice/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
