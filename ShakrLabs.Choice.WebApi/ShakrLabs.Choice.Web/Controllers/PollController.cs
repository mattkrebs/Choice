using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ShakrLabs.Choice.Data;

namespace ShakrLabs.Choice.Web.Controllers
{
    public class PollController : ApiController
    {
        private ChoiceAppEntities db = new ChoiceAppEntities();

        // GET api/Poll
        public IEnumerable<Poll> GetPolls()
        {
            var polls = db.Polls.Include(p => p.Category).Include(p => p.User);
            return polls.AsEnumerable();
        }

        // GET api/Poll/5
        public Poll GetPoll(Guid id)
        {
            Poll poll = db.Polls.Find(id);
            if (poll == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return poll;
        }

        // PUT api/Poll/5
        public HttpResponseMessage PutPoll(Guid id, Poll poll)
        {
            if (ModelState.IsValid && id == poll.PollId)
            {
                db.Entry(poll).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/Poll
        public HttpResponseMessage PostPoll(Poll poll)
        {
            if (ModelState.IsValid)
            {
                db.Polls.Add(poll);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, poll);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = poll.PollId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Poll/5
        public HttpResponseMessage DeletePoll(Guid id)
        {
            Poll poll = db.Polls.Find(id);
            if (poll == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Polls.Remove(poll);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, poll);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}