using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using System.Runtime.Serialization;

namespace Choice.Core.Models
{
    public class ChoiceItem 
    {
        
        public int Id { get; set; }        
        public String Category { get; set; }        
       
        
        
        [IgnoreDataMember]
        public List<ChoiceImage> Images { get; set; }
        
    }

	public class ChoiceImage{
        public int Id { get; set; }
        public int ChoiceId { get; set; }
        public String ImageUrl { get; set; }
        public String SAS { get; set; }
        
        [IgnoreDataMember]
        public byte[] ImageStream { get; set; }

	
	}
}