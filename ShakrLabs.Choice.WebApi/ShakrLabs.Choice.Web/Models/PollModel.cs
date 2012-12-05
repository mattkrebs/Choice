using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShakrLabs.Choice.Data;

namespace ShakrLabs.Choice.Web.Models
{
    public class PollModel
    {

        public Guid PollId { get; set; }
        public byte Cat { get; set; }
        public IEnumerable<PollItemModel> items { get; set; }
        public Int32 TotRats
        {
            get
            {
                Int32 count = 0;
                if (items != null && items.Count() > 0)
                {
                    count = items.Sum(x => x.Count);
                }
                return count;

            }
        }
        
        public PollModel()
        {
            List<PollItemModel> pollItems = new List<PollItemModel>();
            items = pollItems.AsEnumerable();
        }

        public PollModel(Poll pollItem)
        {
            this.PollId = pollItem.PollId;
            this.Cat = pollItem.CategoryId;
            List<PollItemModel> pollItems = new List<PollItemModel>();
            if (pollItem.PollItems.Count == 2)
            {
                pollItems.Add(new PollItemModel(pollItem.PollItems.ToList()[0]));
                pollItems.Add(new PollItemModel(pollItem.PollItems.ToList()[1]));
            }
            items = pollItems.AsEnumerable();
        }


    }
}