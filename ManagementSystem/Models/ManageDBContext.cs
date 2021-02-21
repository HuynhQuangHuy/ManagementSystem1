using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ManagementSystem.Models
{
    public class ManageDBContext : IdentityDbContext<ApplicationUser>
    {
        public ManageDBContext()
                : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Manage> Manages { get; set; }
        public DbSet<ManageUser> User { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
    
}