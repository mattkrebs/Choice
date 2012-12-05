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
        public PollItemModel I1 { get; set; }
        public PollItemModel I2 { get; set; }
        public Int32 TotRats { get; set; }
        public Int32 I1Rats { get; set; }
        public Int32 I2Rats { get; set; }

        public PollModel()
        {
        }

        public PollModel(Poll pollItem)
        {
            this.PollId = pollItem.PollId;
            this.Cat = pollItem.CategoryId;
            if (pollItem.PollItems.Count == 2)
            {
                this.I1 = new PollItemModel(pollItem.PollItems.ToList()[0]);
                this.I2 = new PollItemModel(pollItem.PollItems.ToList()[1]);
            }

        }


    }
}