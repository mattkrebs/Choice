using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShakrLabs.Choice.Data;

namespace ShakrLabs.Choice.Data.Models
{
    public class Category 
    {

        public byte Id { get; set; }
        public string Name { get; set; }
        public byte Rank { get; set; }


        #region IChoiceRepository Members

        //private ChoiceAppEntities db = new ChoiceAppEntities();

        public static IEnumerable<Category> GetAll()
        {
            ChoiceAppEntities db = new ChoiceAppEntities();

            List<Category> model = new List<Category>();
           var items = db.Categories.ToList();

           foreach (var item in items)
           {
               Category c = new Category()
               {
                   Id = item.CategoryId,
                   Name = item.CategoryName,
                   Rank = item.Rank.GetValueOrDefault(0)
               };

               model.Add(c);

           }


           return model.OrderBy(x => x.Rank).ThenBy(y => y.Name).ToList();
        }

        
        #endregion
    }
}