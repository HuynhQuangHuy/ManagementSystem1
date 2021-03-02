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

        public DbSet<Course> Courses { get; set; }

        public DbSet<Topic> Topics { get; set; }

        public DbSet<TraineeProfile> TraineeProfiles { get; set; }
        public DbSet<TraineeToCourse> TraineeToCourses { get; set; }
        public DbSet<TrainerProfile> TrainerProfiles { get; set; }
        public DbSet<TrainerToTopic> TrainerToTopics { get; set; }

        public DbSet<TrainerToCourse> TrainerToCourses { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
    
}