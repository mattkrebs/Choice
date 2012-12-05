using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ShakrLabs.Choice.Data;
using ShakrLabs.Choice.Web.Models;
using System.Net;
using System.Net.Http;


namespace ShakrLabs.Choice.Web.Controllers
{
    public class PublicController : ApiController
    {
        //
        // GET: /Public/
        private ChoiceAppEntities db = new ChoiceAppEntities();

        public PollResponseModel GetNew()
        {
            PollResponseModel prm =  new PollResponseModel();
            //prm.Add(poll);
            //prm.Polls.Add(new PollModel());

            return prm;
        }


        public PollResponseModel GetAll()
        {
            var polls = db.Polls.ToList();
            return new PollResponseModel(polls);

            //return "is alive";
        }
        public String GetElvis()
        {
            return "is alive";
        }

        private User CreateUser()
        {
            User newUser = new User();
            newUser.UserId = Guid.NewGuid();
            newUser.CreatedDate = DateTime.Now;
            newUser.Active = true;
            db.Users.Add(newUser);
            db.SaveChanges();

            return newUser;
        }
        public PollResponseModel GetBatch(string token)
        {

            Guid checkGuid = new Guid();
            User user = new User();
            if (!Guid.TryParse(token, out checkGuid))
            {
                checkGuid = Guid.Empty;
            }

            if (checkGuid == Guid.Empty)
            {
                user = CreateUser();
            }
            else
            {
                //check if user exists
                user = db.Users.Find(checkGuid);
                if (user == null || user.UserId == Guid.Empty)
                {
                    user = CreateUser();
                }
            }

            var polls = db.Polls.ToList();
            PollResponseModel prm =  new PollResponseModel(polls);
            prm.MemberId = user.UserId;
            return prm;
        }

        //[System.Web.Mvc.HttpGet]
        public HttpResponseMessage GetAddRating(string token, Guid itemId)
        {
            //parse batch id to get user id from it
            Guid checkGuid = new Guid();
            User user = new User();
            if (!Guid.TryParse(token, out checkGuid))
            {
                checkGuid = Guid.Empty;
            }

            //get item
            Rating rating = new Rating();
            rating.CreatedDate = DateTime.Now;
            rating.RatingId = Guid.NewGuid();
            rating.PollItemId = itemId;
            rating.UserId = checkGuid;
            db.Ratings.Add(rating);
            db.SaveChanges();

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, rating);
            //response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = poll.PollId }));
            return response;

            //return new PollResponseModel();
        }

        //public PollResponseModel GetNextPolls(string token, double lat, double lng, int MaxReturn)
        //{

        //    PollRequestModel req = new PollRequestModel();

        //    req.MaxReturn = MaxReturn;
        //    req.lat = lat;
        //    req.lng = lng;
        //    req.Token = token;
        //    return GetNextPollsRequest(req);
            
        //}


        //IS this needed
        //protected override void Dispose(bool disposing)
        //{
        //    if (db != null)
        //    {
        //        db.Dispose();
        //        base.Dispose(disposing);
        //    }
        //}
    }
}
