namespace MSWD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PWDEA3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pwds", "EducationalAttainment", c => c.String());
            DropColumn("dbo.Pwds", "EAElementary");
            DropColumn("dbo.Pwds", "EAElementaryUndergraduate");
            DropColumn("dbo.Pwds", "EAHighSchool");
            DropColumn("dbo.Pwds", "EAHighSchoolUndergraduate");
            DropColumn("dbo.Pwds", "EACollege");
            DropColumn("dbo.Pwds", "EACollegeUndergraduate");
            DropColumn("dbo.Pwds", "EAGraduate");
            DropColumn("dbo.Pwds", "EASPED");
            DropColumn("dbo.Pwds", "EAPostGraduate");
            DropColumn("dbo.Pwds", "EAVocational");
            DropColumn("dbo.Pwds", "EANone");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pwds", "EANone", c => c.Boolean());
            AddColumn("dbo.Pwds", "EAVocational", c => c.Boolean());
            AddColumn("dbo.Pwds", "EAPostGraduate", c => c.Boolean());
            AddColumn("dbo.Pwds", "EASPED", c => c.Boolean());
            AddColumn("dbo.Pwds", "EAGraduate", c => c.Boolean());
            AddColumn("dbo.Pwds", "EACollegeUndergraduate", c => c.Boolean());
            AddColumn("dbo.Pwds", "EACollege", c => c.Boolean());
            AddColumn("dbo.Pwds", "EAHighSchoolUndergraduate", c => c.Boolean());
            AddColumn("dbo.Pwds", "EAHighSchool", c => c.Boolean());
            AddColumn("dbo.Pwds", "EAElementaryUndergraduate", c => c.Boolean());
            AddColumn("dbo.Pwds", "EAElementary", c => c.Boolean());
            DropColumn("dbo.Pwds", "EducationalAttainment");
        }
    }
}
