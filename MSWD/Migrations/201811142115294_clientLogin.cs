namespace MSWD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class clientLogin : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Clients", "CreatedByUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "CityId", "dbo.Cities");
            DropIndex("dbo.Clients", new[] { "CreatedByUserId" });
            DropIndex("dbo.AspNetUsers", new[] { "CityId" });
            AddColumn("dbo.AspNetUsers", "ClientId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "CreatedByUserId", c => c.String());
            AddColumn("dbo.AspNetUsers", "CreatedBy_Id", c => c.String(maxLength: 128));
            AlterColumn("dbo.AspNetUsers", "CityId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "CityId");
            CreateIndex("dbo.AspNetUsers", "ClientId");
            CreateIndex("dbo.AspNetUsers", "CreatedBy_Id");
            AddForeignKey("dbo.AspNetUsers", "ClientId", "dbo.Clients", "ClientId");
            AddForeignKey("dbo.AspNetUsers", "CreatedBy_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.AspNetUsers", "CityId", "dbo.Cities", "CityId");
            DropColumn("dbo.Clients", "CreatedByUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clients", "CreatedByUserId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.AspNetUsers", "CityId", "dbo.Cities");
            DropForeignKey("dbo.AspNetUsers", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "ClientId", "dbo.Clients");
            DropIndex("dbo.AspNetUsers", new[] { "CreatedBy_Id" });
            DropIndex("dbo.AspNetUsers", new[] { "ClientId" });
            DropIndex("dbo.AspNetUsers", new[] { "CityId" });
            AlterColumn("dbo.AspNetUsers", "CityId", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "CreatedBy_Id");
            DropColumn("dbo.AspNetUsers", "CreatedByUserId");
            DropColumn("dbo.AspNetUsers", "ClientId");
            CreateIndex("dbo.AspNetUsers", "CityId");
            CreateIndex("dbo.Clients", "CreatedByUserId");
            AddForeignKey("dbo.AspNetUsers", "CityId", "dbo.Cities", "CityId", cascadeDelete: true);
            AddForeignKey("dbo.Clients", "CreatedByUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
