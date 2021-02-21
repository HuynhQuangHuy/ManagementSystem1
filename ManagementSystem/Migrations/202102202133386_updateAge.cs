namespace ManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAge : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Manages", "Age", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Manages", "Age", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
