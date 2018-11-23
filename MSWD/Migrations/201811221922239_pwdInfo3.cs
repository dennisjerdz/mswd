namespace MSWD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pwdInfo3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "School", c => c.String());
            AddColumn("dbo.Clients", "EducationalAttainment", c => c.String());
            AddColumn("dbo.Clients", "TypeOfSkill", c => c.String());
            DropColumn("dbo.Pwds", "School");
            DropColumn("dbo.Pwds", "EducationalAttainment");
            DropColumn("dbo.Pwds", "TypeOfSkill");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pwds", "TypeOfSkill", c => c.String());
            AddColumn("dbo.Pwds", "EducationalAttainment", c => c.String());
            AddColumn("dbo.Pwds", "School", c => c.String());
            DropColumn("dbo.Clients", "TypeOfSkill");
            DropColumn("dbo.Clients", "EducationalAttainment");
            DropColumn("dbo.Clients", "School");
        }
    }
}
