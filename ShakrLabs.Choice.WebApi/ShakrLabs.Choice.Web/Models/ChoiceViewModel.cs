using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShakrLabs.Choice.Web.Models
{
    public class ChoiceViewModel
    {

        public Guid PollId { get; set; }
        public string ImageUrl1 { get; set; }
        public string ImageUrl2 { get; set; }
        public HttpPostedFileBase File1 { get; set; }
        public HttpPostedFileBase File2 { get; set; }
        public byte CategoryId { get; set; }
        public Guid MemberId { get; set; }

        public SelectList GetCategories()
        {
            List<Category> cats = new List<Category>(Category.GetAll());

            SelectList list = new SelectList(cats,"Id","Name");
            return list;
        }
    
    

    }
}