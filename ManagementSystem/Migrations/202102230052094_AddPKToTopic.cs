namespace ManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPKToTopic : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TrainerToTopics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TrainerId = c.String(nullable: false, maxLength: 128),
                        TopicId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Topics", t => t.TopicId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.TrainerId, cascadeDelete: true)
                .Index(t => t.TrainerId)
                .Index(t => t.TopicId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrainerToTopics", "TrainerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.TrainerToTopics", "TopicId", "dbo.Topics");
            DropIndex("dbo.TrainerToTopics", new[] { "TopicId" });
            DropIndex("dbo.TrainerToTopics", new[] { "TrainerId" });
            DropTable("dbo.TrainerToTopics");
        }
    }
}
