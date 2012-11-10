using ShakrLabs.Choice.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakrLabs.Choice.Web.Models
{
    public class PollResponseModel
    {

        public byte Category { get; set; }
        public IEnumerable<PollItemResponseModel> Urls { get; set; }
        public Guid PollID { get; set; }
        public Guid MemberId { get; set; }


        public PollResponseModel(Poll poll)
        {
            this.Category = poll.CategoryId;
            this.MemberId = poll.UserId;
            this.PollID = poll.PollId;
            List<PollItemResponseModel> urls = new List<PollItemResponseModel>();
            List<PollItem> items = poll.PollItems.ToList();
            foreach (var item in items)
            {
                urls.Add(new PollItemResponseModel(item));
            }
            this.Urls = urls;
        }
      
    }
}