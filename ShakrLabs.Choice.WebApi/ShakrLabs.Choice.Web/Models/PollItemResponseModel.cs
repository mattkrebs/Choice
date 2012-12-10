using ShakrLabs.Choice.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakrLabs.Choice.Data.Models
{
    public class PollItemResponseModel
    {
        public string PollItemId { get; set; }
        public string PollUrl { get; set; }



        public PollItemResponseModel(PollItem pollItem)
        {
            this.PollItemId = pollItem.PollItemId.ToString();
            this.PollUrl = pollItem.ImageUrl;

        }
    }
}