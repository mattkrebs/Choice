using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Choice.WebApi.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public User CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
