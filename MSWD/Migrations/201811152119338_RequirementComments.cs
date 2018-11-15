namespace MSWD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequirementComments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RequirementComments",
                c => new
                    {
                        RequirementCommentId = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        DateTimeCreated = c.DateTime(nullable: false),
                        RequirementId = c.Int(nullable: false),
                        CreatedById = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RequirementCommentId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedById)
                .ForeignKey("dbo.Requirements", t => t.RequirementId, cascadeDelete: true)
                .Index(t => t.RequirementId)
                .Index(t => t.CreatedById);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RequirementComments", "RequirementId", "dbo.Requirements");
            DropForeignKey("dbo.RequirementComments", "CreatedById", "dbo.AspNetUsers");
            DropIndex("dbo.RequirementComments", new[] { "CreatedById" });
            DropIndex("dbo.RequirementComments", new[] { "RequirementId" });
            DropTable("dbo.RequirementComments");
        }
    }
}
