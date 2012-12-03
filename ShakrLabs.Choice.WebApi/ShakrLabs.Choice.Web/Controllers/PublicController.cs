using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ShakrLabs.Choice.Data;
using ShakrLabs.Choice.Web.Models;

namespace ShakrLabs.Choice.Web.Controllers
{
    public class PublicController : ApiController
    {
        //
        // GET: /Public/

        //private ChoiceAppEntities db = new ChoiceAppEntities();


        public PollResponseModel GetNew()
        {
            return new PollResponseModel();
        }

        public PollResponseModel GetNextPollsRequest(PollRequestModel request)
        {

            PollResponseModel prm = new PollResponseModel();

            return prm;
        }

        public PollResponseModel GetNextPolls(string token, double lat, double lng, int MaxReturn)
        {

            PollRequestModel req = new PollRequestModel();

            req.MaxReturn = MaxReturn;
            req.lat = lat;
            req.lng = lng;
            req.Token = token;
            return GetNextPollsRequest(req);
            
        }


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
