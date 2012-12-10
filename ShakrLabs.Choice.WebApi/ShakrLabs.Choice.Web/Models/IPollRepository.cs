using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakrLabs.Choice.Data.Models
{
    public interface IPollRepository
    {
        IEnumerable<PollResponseModel> Get();
        PollResponseModel Add(PollRequestModel poll);
        PollResponseModel Get(string id);
        bool Delete(string id);
    }
}