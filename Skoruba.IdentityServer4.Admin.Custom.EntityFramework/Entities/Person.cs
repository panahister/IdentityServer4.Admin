using Skoruba.IdentityServer4.Admin.EntityFramework.Shared.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Custom.EntityFramework.Entities
{
    public class Person
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("UserIdentity")]
        public string UserId { get; set; }
        public UserIdentity UserIdentity { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public Gender Gender { get; set; }
    }
}
