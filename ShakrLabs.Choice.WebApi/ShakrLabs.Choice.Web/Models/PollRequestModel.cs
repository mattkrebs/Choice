using ShakrLabs.Choice.Data;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;

namespace ShakrLabs.Choice.Data.Models
{
    public class PollRequestModel
    {
        //public IEnumerable<HttpPostedFileBase> Files { get; set; }
        public byte Category { get; set; }
        //public Guid PollID { get; set; }
        //public int MemberId { get; set; }
        public string Token { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public int MaxReturn { get; set; }

        public PollRequestModel()
        {
            this.Token = String.Empty;
        }

    }
}