using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Custom.EntityFramework.Entities
{
    public class Permission 
    {
        [Key]
        public long Id { get; set; }

        [ForeignKey("Resource")]
        public long ResourceId { get; set; }
        public Resource Resource { get; set; }

        [ForeignKey("UserIdentityRole")]
        public string RoleId { get; set; }
        public UserIdentityRole UserIdentityRole { get; set; }
      
        public PermissionStatus PermissionStatus { get; set; }
        public PermissionAttribute PermissionAttribute { get; set; }




    }
}
