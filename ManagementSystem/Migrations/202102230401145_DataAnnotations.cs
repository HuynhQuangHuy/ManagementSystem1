namespace ManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataAnnotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TraineeProfiles", "Full_Name", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.TrainerProfiles", "Full_Name", c => c.String(nullable: false, maxLength: 60));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TrainerProfiles", "Full_Name", c => c.String());
            AlterColumn("dbo.TraineeProfiles", "Full_Name", c => c.String());
        }
    }
}
