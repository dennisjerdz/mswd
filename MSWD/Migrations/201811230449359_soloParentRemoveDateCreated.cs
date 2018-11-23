namespace MSWD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class soloParentRemoveDateCreated : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SoloParents", "DateCreated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SoloParents", "DateCreated", c => c.DateTime(nullable: false));
        }
    }
}
