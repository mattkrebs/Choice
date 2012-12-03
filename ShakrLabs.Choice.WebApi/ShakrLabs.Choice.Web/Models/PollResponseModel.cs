using ShakrLabs.Choice.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakrLabs.Choice.Web.Models
{
    public class PollResponseModel
    {

     //   public byte Category { get; set; }
        public List<PollModel> Polls { get; private set; }
       // public Guid PollID { get; set; }
        //public Guid MemberId { get; set; }

        //public IEnumerable<Poll>

        public void Add(Poll item)
        {
            PollModel poll = new PollModel();
            poll.Category = item.CategoryId;
            poll.PollId = poll.PollId;
            //this.MemberId = poll.UserId;
            
            if(item.PollItems.Count == 2)
            {
                poll.Item1 = new PollItemModel(item.PollItems.ToList()[0]);
                poll.Item2 = new PollItemModel(item.PollItems.ToList()[1]);
            }
            this.Polls.Add(poll);
        }

        public PollResponseModel()
        {
            this.Polls = new List<PollModel>();
        }
    }
}