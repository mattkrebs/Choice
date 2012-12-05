using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShakrLabs.Choice.Data;

namespace ShakrLabs.Choice.Web.Models
{
    public class PollItemModel
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        //public Decimal Rating { get; set; }
        public Int32 Count { get; set; }

        public PollItemModel()
        {

        }

        public PollItemModel(PollItem item)
        {
            this.Id = item.PollItemId;
            this.Url = item.ImageUrl;
        }


    }
}
