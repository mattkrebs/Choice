//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShakrLabs.Choice.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Category
    {
        public Category()
        {
            this.Polls = new HashSet<Poll>();
        }
    
        public byte CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Nullable<int> SubCategoryId { get; set; }
        public bool Active { get; set; }
        public Nullable<byte> Rank { get; set; }
    
        public virtual ICollection<Poll> Polls { get; set; }
    }
}
