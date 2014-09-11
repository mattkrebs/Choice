using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choice.Data
{
    public class ChoiceItem
    {
        public Guid ChoiceItemId { get; set; }
        public Group GroupId { get; set; }
        public string ImageUrl { get; set; }
        public string Tags { get; set; }
        public User User { get; set; }

    }
}
