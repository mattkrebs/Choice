using ShakrLabs.Choice.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakrLabs.Choice.Web.Models
{
    public class PollRequestModel
    {

        
        public IEnumerable<HttpPostedFileBase> Files { get; set; }
        public int Category { get; set; }
        public Guid PollID { get; set; }
        public int MemberId { get; set; }


    }
}