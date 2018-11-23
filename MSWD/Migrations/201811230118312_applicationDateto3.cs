namespace MSWD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class applicationDateto3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pwds", "ApplicationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Pwds", "ApprovalDate", c => c.DateTime());
            AddColumn("dbo.SeniorCitizens", "ApplicationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SeniorCitizens", "ApprovalDate", c => c.DateTime());
            AddColumn("dbo.SoloParents", "ApplicationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.SoloParents", "ApprovalDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoloParents", "ApprovalDate");
            DropColumn("dbo.SoloParents", "ApplicationDate");
            DropColumn("dbo.SeniorCitizens", "ApprovalDate");
            DropColumn("dbo.SeniorCitizens", "ApplicationDate");
            DropColumn("dbo.Pwds", "ApprovalDate");
            DropColumn("dbo.Pwds", "ApplicationDate");
        }
    }
}
