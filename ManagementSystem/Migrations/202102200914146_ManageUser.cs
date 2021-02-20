namespace ManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManageUser : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ManageUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ManageId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Manages", t => t.ManageId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ManageId)
                .Index(t => t.UserId);
            
            
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.ManageUsers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ManageUsers", "ManageId", "dbo.Manages");
            DropIndex("dbo.ManageUsers", new[] { "UserId" });
            DropIndex("dbo.ManageUsers", new[] { "ManageId" });
            DropTable("dbo.ManageUsers");
        }
    }
}
