namespace ManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTrainerProfileTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrainerProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TrainerId = c.String(nullable: false, maxLength: 128),
                        Full_Name = c.String(),
                        External_Internal = c.String(),
                        Education = c.String(),
                        Working_Place = c.String(),
                        Phone_Number = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.TrainerId, cascadeDelete: true)
                .Index(t => t.TrainerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrainerProfiles", "TrainerId", "dbo.AspNetUsers");
            DropIndex("dbo.TrainerProfiles", new[] { "TrainerId" });
            DropTable("dbo.TrainerProfiles");
        }
    }
}
