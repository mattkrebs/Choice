using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShakrLabs.Choice.Data;

namespace ShakrLabs.Choice.Web.Models
{
    public class PollItemModel
    {
        public Guid PollItemId { get; set; }
        public string PollUrl { get; set; }
        public Decimal Rating { get; set; }

        public PollItemModel()
        {

        }

        public PollItemModel(PollItem item)
        {
            this.PollItemId = item.PollItemId;
            this.PollUrl = item.ImageUrl;
        }


    }
}