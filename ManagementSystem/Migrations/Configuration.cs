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
            context.Manages.AddOrUpdate(i => i.Name,
                new Manage
                {
                    Name = "Mary",
                    Date = DateTime.Parse("2000-1-11"),
                    Role = "Student",
                    Class = "MG",
                    Age = 7.99M
                },

                 new Manage
                 {
                     Name = "Gin",
                     Date = DateTime.Parse("2000-1-11"),
                     Role = "Student",
                     Class= "PG",
                     Age = 7.99M
                 },

                 new Manage
                 {
                     Name = "Conan",
                     Date = DateTime.Parse("2000-1-11"),
                     Role = "Student",
                     Class = "PG",
                     Age = 7.99M
                 },

               new Manage
               {
                   Name = "Ran",
                   Date = DateTime.Parse("2000-1-11"),
                   Role = "Student",
                   Class = "PG",
                   Age = 17
               }
           );

        }
    }
}
