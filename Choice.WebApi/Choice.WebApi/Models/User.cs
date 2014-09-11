using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Choice.WebApi.Models
{
    public class User
    {
        public int UserId { get; set; }
        public int SocialId { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        
    }
}
