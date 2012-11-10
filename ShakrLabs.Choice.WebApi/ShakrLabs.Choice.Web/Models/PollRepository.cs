using ShakrLabs.Choice.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace ShakrLabs.Choice.Web.Models
{
    public class PollRepository : IPollRepository
    {

        private ChoiceAppEntities db = new ChoiceAppEntities();

       

        public IEnumerable<PollResponseModel> Get()
        {

              List<PollResponseModel> polls = new List<PollResponseModel>();
            foreach (var item in db.Polls)
	        {
                polls.Add(new PollResponseModel(item));
	        }
            return polls;
          
        }

        public PollResponseModel Get(string id)
        {
            Poll poll = db.Polls.Find(new Guid(id));
            if (poll == null)
            {
                return null;
            }
            return new PollResponseModel(poll);
        }

        public PollResponseModel Add(PollRequestModel poll){

            return null;
        }




       

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }


    }
}