using System;
using System.Collections.Generic;
using System.Text;

namespace Choice.Services.Shared.Models
{
    public class ChoiceItem
    {
        public System.Guid ChoiceId { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string User_Id { get; set; }


        public virtual Option Option1 { get; set; }
        public virtual Option Option2 { get; set; }



    }
}
