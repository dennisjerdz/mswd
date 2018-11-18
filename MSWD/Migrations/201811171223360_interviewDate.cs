namespace MSWD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class interviewDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pwds", "InterviewDate", c => c.DateTime());
            AddColumn("dbo.SeniorCitizens", "InterviewDate", c => c.DateTime());
            AddColumn("dbo.SoloParents", "InterviewDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoloParents", "InterviewDate");
            DropColumn("dbo.SeniorCitizens", "InterviewDate");
            DropColumn("dbo.Pwds", "InterviewDate");
        }
    }
}
