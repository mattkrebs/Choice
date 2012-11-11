using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShakrLabs.Choice.Data;

namespace ShakrLabs.Choice.Web.Controllers
{
    public class WebPollController : Controller
    {
        private ChoiceAppEntities db = new ChoiceAppEntities();


        //[HttpPost]
        //public ActionResult Create(PollRequestModel model)
        //{
        //    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
        //       CloudConfigurationManager.GetSetting("StorageConnectionString"));

        //    // Create the blob client     
        //    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

        //    // Retrieve a reference to a container 
        //    CloudBlobContainer container = blobClient.GetContainerReference("images");

        //    // Create the container if it doesn't already exist
        //    container.CreateIfNotExist();
        //    Poll poll = new Poll()
        //    {
        //        PollId = Guid.NewGuid(),
        //        Active = true,
        //        CategoryId = model.Category,
        //        CreatedDate = DateTime.Now,
        //        UserId = 1
        //    };
        //    foreach (var file in model.Files)
        //    {
        //        try
        //        {
        //            var stream = file.InputStream;


        //            CloudBlob blob = container.GetBlobReference(file.FileName);
        //            blob.UploadFromStream(stream);
        //            //Create Poll

        //            poll.PollItems.Add(new PollItem()
        //            {
        //                PollItemId = Guid.NewGuid(),
        //                Active = true,
        //                CreatedDate = DateTime.Now,
        //                ImageUrl = blob.Uri.OriginalString,
        //                UserId = 1
        //            });



        //            //blob.UploadFile(file);
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { success = false, message = ex.Message }, "application/json");
        //        }
        //    }
        //    db.Polls.Add(poll);
        //    db.SaveChanges();
        //    return Json(new { success = true }, "text/html");


        //}
        //
        // GET: /WebPoll/

        public ActionResult Index()
        {
            var polls = db.Polls.Include(p => p.Category).Include(p => p.User);
            return View(polls.ToList());
        }

        //
        // GET: /WebPoll/Details/5

        public ActionResult Details(Guid id)
        {
            Poll poll = db.Polls.Find(id);
            if (poll == null)
            {
                return HttpNotFound();
            }
            return View(poll);
        }

        //
        // GET: /WebPoll/Create

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FacebookId");
            return View();
        }

        //
        // POST: /WebPoll/Create

        [HttpPost]
        public ActionResult Create(Poll poll)
        {
            if (ModelState.IsValid)
            {
                poll.PollId = Guid.NewGuid();
                db.Polls.Add(poll);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", poll.CategoryId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FacebookId", poll.UserId);
            return View(poll);
        }

        //
        // GET: /WebPoll/Edit/5

        public ActionResult Edit(Guid id)
        {
            Poll poll = db.Polls.Find(id);
            if (poll == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", poll.CategoryId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FacebookId", poll.UserId);
            return View(poll);
        }

        //
        // POST: /WebPoll/Edit/5

        [HttpPost]
        public ActionResult Edit(Poll poll)
        {
            if (ModelState.IsValid)
            {
                db.Entry(poll).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", poll.CategoryId);
            ViewBag.UserId = new SelectList(db.Users, "UserId", "FacebookId", poll.UserId);
            return View(poll);
        }

        //
        // GET: /WebPoll/Delete/5

        public ActionResult Delete(Guid id )
        {
            Poll poll = db.Polls.Find(id);
            if (poll == null)
            {
                return HttpNotFound();
            }
            return View(poll);
        }

        //
        // POST: /WebPoll/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Poll poll = db.Polls.Find(id);
            db.Polls.Remove(poll);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}