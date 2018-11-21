namespace MSWD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PWDEA2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pwds", "EAElementaryUndergraduate", c => c.Boolean());
            AddColumn("dbo.Pwds", "EAHighSchool", c => c.Boolean());
            AddColumn("dbo.Pwds", "EACollege", c => c.Boolean());
            AddColumn("dbo.Pwds", "EACollegeUndergraduate", c => c.Boolean());
            AddColumn("dbo.Pwds", "EAGraduate", c => c.Boolean());
            AddColumn("dbo.Pwds", "EASPED", c => c.Boolean());
            AddColumn("dbo.Pwds", "EAPostGraduate", c => c.Boolean());
            AddColumn("dbo.Pwds", "EAVocational", c => c.Boolean());
            AddColumn("dbo.Pwds", "EANone", c => c.Boolean());
            DropColumn("dbo.Pwds", "Graduate");
            DropColumn("dbo.Pwds", "SPED");
            DropColumn("dbo.Pwds", "ElementaryUnderGraduate");
            DropColumn("dbo.Pwds", "College");
            DropColumn("dbo.Pwds", "PostGraduate");
            DropColumn("dbo.Pwds", "HighSchool");
            DropColumn("dbo.Pwds", "CollegeUndergraduate");
            DropColumn("dbo.Pwds", "Vocational");
            DropColumn("dbo.Pwds", "None");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pwds", "None", c => c.Boolean());
            AddColumn("dbo.Pwds", "Vocational", c => c.Boolean());
            AddColumn("dbo.Pwds", "CollegeUndergraduate", c => c.Boolean());
            AddColumn("dbo.Pwds", "HighSchool", c => c.Boolean());
            AddColumn("dbo.Pwds", "PostGraduate", c => c.Boolean());
            AddColumn("dbo.Pwds", "College", c => c.Boolean());
            AddColumn("dbo.Pwds", "ElementaryUnderGraduate", c => c.Boolean());
            AddColumn("dbo.Pwds", "SPED", c => c.Boolean());
            AddColumn("dbo.Pwds", "Graduate", c => c.Boolean());
            DropColumn("dbo.Pwds", "EANone");
            DropColumn("dbo.Pwds", "EAVocational");
            DropColumn("dbo.Pwds", "EAPostGraduate");
            DropColumn("dbo.Pwds", "EASPED");
            DropColumn("dbo.Pwds", "EAGraduate");
            DropColumn("dbo.Pwds", "EACollegeUndergraduate");
            DropColumn("dbo.Pwds", "EACollege");
            DropColumn("dbo.Pwds", "EAHighSchool");
            DropColumn("dbo.Pwds", "EAElementaryUndergraduate");
        }
    }
}