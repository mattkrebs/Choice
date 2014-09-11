using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choice.WebApi.Models
{
    public class Question
    {
        public Guid QuestionId { get; set; }
        public Question Question1 { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public User CreatedBy { get; set; }


    }
}
