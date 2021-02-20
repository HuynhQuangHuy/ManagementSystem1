namespace ManagementSystem.Migrations
{
    using ManagementSystem.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ManagementSystem.Models.ManageDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ManagementSystem.Models.ManageDBContext context)
        {
      
        }
    }
}
