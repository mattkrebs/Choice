using ShakrLabs.Choice.Data;
using ShakrLabs.Choice.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ShakrLabs.Choice.Web.Controllers
{
    public class PollController : ApiController
    {

        IPollRepository repository;

        public PollController(IPollRepository repository) 
        {
            this.repository = repository;
        }

        #region GET
        // GET api/poll
        public IEnumerable<PollResponseModel> GetPolls()
        {
            
            return repository.Get();
        }


        // GET api/poll/5
        public PollResponseModel Get(string id)
        {
            PollResponseModel poll = repository.Get(id);
            if (poll == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

            return poll;
        }
        #endregion

        // POST api/poll
        public void Post([FromBody]string value)
        {
        }

        // PUT api/poll/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/poll/5
        public void Delete(int id)
        {
        }
    }
}
