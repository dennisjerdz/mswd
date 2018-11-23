namespace MSWD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class releaseDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pwds", "ReleaseDate", c => c.DateTime());
            AddColumn("dbo.SeniorCitizens", "ReleaseDate", c => c.DateTime());
            AddColumn("dbo.SoloParents", "ReleaseDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SoloParents", "ReleaseDate");
            DropColumn("dbo.SeniorCitizens", "ReleaseDate");
            DropColumn("dbo.Pwds", "ReleaseDate");
        }
    }
}
