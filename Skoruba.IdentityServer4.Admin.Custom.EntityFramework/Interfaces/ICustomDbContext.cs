using Microsoft.EntityFrameworkCore;
using Custom.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Custom.EntityFramework.Interfaces
{
    public interface ICustomDbContext
    {
        DbSet<Resource> Resources { get; set; }
        DbSet<Permission> Permissions { get; set; }
    }
}
