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
        public IEnumerable<PollModel> Polls
        {
            get
            {
                return _PollList == null ? null : _PollList.AsEnumerable();
            }
        }

        private List<PollModel> _PollList { get; set; }

        public string BatchId { get; set; }
        // public Guid PollID { get; set; }
        public Guid MemberId { get; set; }

        //public IEnumerable<Poll>

        public void Add(Poll item)
        {

            PollModel poll = new PollModel();
            poll.Cat = item.CategoryId;
            poll.PollId = poll.PollId;
            //this.MemberId = poll.UserId;

            
            this._PollList.Add(poll);
        }


        public PollResponseModel(List<Poll> polls)
        {
            
            this._PollList = new List<PollModel>();
            if(polls != null)
            {
                foreach(Poll p in polls)
                {
                    this._PollList.Add(new PollModel(p));
                }

            }
            this.BatchId = Guid.NewGuid().ToString();
        }
        public PollResponseModel()
        {
            this._PollList = new List<PollModel>();
            this.BatchId = Guid.NewGuid().ToString();
        }
    }
}