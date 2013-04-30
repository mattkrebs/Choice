using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Choice.Core
{
    public class ViewModel<T>
    {

        public Action<string, string> ShowAlert { get; set; }


        protected List<T> items = new List<T>();
        public List<T> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
            }
        }


        
    }
}
