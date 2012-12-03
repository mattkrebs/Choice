using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShakrLabs.Choice.Web.Models
{
    public class PollModel
    {

        public Guid PollId { get; set; }
        public byte Category { get; set; }
        public PollItemModel Item1 { get; set; }
        public PollItemModel Item2 { get; set; }
        public Int32 TotalRatings { get; set; }
        public Int32 Item1Ratings { get; set; }
        public Int32 Item2Ratings { get; set; }


    }
}