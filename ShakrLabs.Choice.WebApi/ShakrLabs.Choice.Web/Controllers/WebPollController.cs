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