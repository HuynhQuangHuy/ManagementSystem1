namespace ManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCategoryFKToTodoModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Manages", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Manages", "CategoryId");
            AddForeignKey("dbo.Manages", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Manages", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Manages", new[] { "CategoryId" });
            DropColumn("dbo.Manages", "CategoryId");
        }
    }
}
