using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Custom.EntityFramework.Entities
{
    public class Resource 
    {

        [Key]
        public long Id { get; set; }
     
        [ForeignKey("Parent")]
        public long? ParentId { get; set; }
        public virtual  Resource Parent { get; set; }
        public virtual ICollection<Resource> Children { get; set; }
        
        public string ResourceTitle { get; set; }

        public string ResourceValue { get; set; }
        public string MobileResourceValue { get; set; }

        public bool Active { get; set; }


        public ResourceType ResourceType { get; set; }
        public string Attribute { get; set; }
        public string MobileAttribute { get; set; }

        public UIResourceType UIResourceType { get; set; }

      

        public bool ShowOnMobile { get; set; }
    }
}
