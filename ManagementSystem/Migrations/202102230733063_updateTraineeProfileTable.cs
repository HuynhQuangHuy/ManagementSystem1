namespace ManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTraineeProfileTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TraineeProfiles", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TraineeProfiles", "Date", c => c.DateTime(nullable: false));
        }
    }
}
