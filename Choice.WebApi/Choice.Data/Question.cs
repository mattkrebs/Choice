using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choice.Data
{
    public class Question
    {
        public Guid QuestionId { get; set; }
        public Question Question1 { get; set; }
        public Question Question2 { get; set; }
        public DateTime CreatedDate { get; set; }
        public User CreatedBy { get; set; }


    }
}
