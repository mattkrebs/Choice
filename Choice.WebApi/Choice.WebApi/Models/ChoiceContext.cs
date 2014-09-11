using Choice.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choice.WebApi.Models
{
    public class ChoiceContext : DbContext
    {

        public ChoiceContext()
            : base("ChoiceContext")
        {

        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<ChoiceItem> Choices { get; set; }
        public DbSet<Question> Questions { get; set; }
       
    }







}
