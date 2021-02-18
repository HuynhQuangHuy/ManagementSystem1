namespace ManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataAnnotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Manages", "Name", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.Manages", "Class", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Manages", "Class", c => c.String(maxLength: 10));
            AlterColumn("dbo.Manages", "Name", c => c.String(maxLength: 60));
        }
    }
}
