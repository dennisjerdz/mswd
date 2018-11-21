namespace MSWD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PwdNewInfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pwds", "PsychosocialMentalDisability", c => c.Boolean());
            AddColumn("dbo.Pwds", "VisualDisability", c => c.Boolean());
            AddColumn("dbo.Pwds", "CommunicationDisability", c => c.Boolean());
            AddColumn("dbo.Pwds", "LearningDisability", c => c.Boolean());
            AddColumn("dbo.Pwds", "OrthopedicDisability", c => c.Boolean());
            AddColumn("dbo.Pwds", "IntellectualDisability", c => c.Boolean());
            AddColumn("dbo.Pwds", "Inborn", c => c.Boolean());
            AddColumn("dbo.Pwds", "Acquired", c => c.Boolean());
            AddColumn("dbo.Pwds", "AgeAcquired", c => c.Int());
            AddColumn("dbo.Pwds", "School", c => c.String());
            AddColumn("dbo.Pwds", "EAElementary", c => c.Boolean());
            AddColumn("dbo.Pwds", "EAHighSchoolUndergraduate", c => c.Boolean());
            AddColumn("dbo.Pwds", "Graduate", c => c.Boolean());
            AddColumn("dbo.Pwds", "SPED", c => c.Boolean());
            AddColumn("dbo.Pwds", "ElementaryUnderGraduate", c => c.Boolean());
            AddColumn("dbo.Pwds", "College", c => c.Boolean());
            AddColumn("dbo.Pwds", "PostGraduate", c => c.Boolean());
            AddColumn("dbo.Pwds", "HighSchool", c => c.Boolean());
            AddColumn("dbo.Pwds", "CollegeUndergraduate", c => c.Boolean());
            AddColumn("dbo.Pwds", "Vocational", c => c.Boolean());
            AddColumn("dbo.Pwds", "None", c => c.Boolean());
            AddColumn("dbo.Pwds", "TypeOfSkill", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pwds", "TypeOfSkill");
            DropColumn("dbo.Pwds", "None");
            DropColumn("dbo.Pwds", "Vocational");
            DropColumn("dbo.Pwds", "CollegeUndergraduate");
            DropColumn("dbo.Pwds", "HighSchool");
            DropColumn("dbo.Pwds", "PostGraduate");
            DropColumn("dbo.Pwds", "College");
            DropColumn("dbo.Pwds", "ElementaryUnderGraduate");
            DropColumn("dbo.Pwds", "SPED");
            DropColumn("dbo.Pwds", "Graduate");
            DropColumn("dbo.Pwds", "EAHighSchoolUndergraduate");
            DropColumn("dbo.Pwds", "EAElementary");
            DropColumn("dbo.Pwds", "School");
            DropColumn("dbo.Pwds", "AgeAcquired");
            DropColumn("dbo.Pwds", "Acquired");
            DropColumn("dbo.Pwds", "Inborn");
            DropColumn("dbo.Pwds", "IntellectualDisability");
            DropColumn("dbo.Pwds", "OrthopedicDisability");
            DropColumn("dbo.Pwds", "LearningDisability");
            DropColumn("dbo.Pwds", "CommunicationDisability");
            DropColumn("dbo.Pwds", "VisualDisability");
            DropColumn("dbo.Pwds", "PsychosocialMentalDisability");
        }
    }
}
