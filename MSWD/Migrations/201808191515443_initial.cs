namespace MSWD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Requirements", "PWD_PWDId", "dbo.PWDs");
            DropForeignKey("dbo.Requirements", "SeniorCitizen_SeniorCitizenId", "dbo.SeniorCitizens");
            DropForeignKey("dbo.Requirements", "SoloParent_SoloParentId", "dbo.SoloParents");
            DropForeignKey("dbo.PWDs", "CityId", "dbo.Cities");
            DropForeignKey("dbo.SeniorCitizens", "CityId", "dbo.Cities");
            DropForeignKey("dbo.SoloParents", "CityId", "dbo.Cities");
            DropIndex("dbo.Pwds", new[] { "CityId" });
            DropIndex("dbo.Requirements", new[] { "PWD_PWDId" });
            DropIndex("dbo.Requirements", new[] { "SeniorCitizen_SeniorCitizenId" });
            DropIndex("dbo.Requirements", new[] { "SoloParent_SoloParentId" });
            DropIndex("dbo.SeniorCitizens", new[] { "CityId" });
            DropIndex("dbo.SoloParents", new[] { "CityId" });
            RenameColumn(table: "dbo.Pwds", name: "CityId", newName: "City_CityId");
            RenameColumn(table: "dbo.SeniorCitizens", name: "CityId", newName: "City_CityId");
            RenameColumn(table: "dbo.SoloParents", name: "CityId", newName: "City_CityId");
            DropPrimaryKey("dbo.SeniorCitizens");
            DropPrimaryKey("dbo.SoloParents");
            DropPrimaryKey("dbo.Pwds");
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ClientId = c.Int(nullable: false, identity: true),
                        CityId = c.Int(nullable: false),
                        GivenName = c.String(),
                        MiddleName = c.String(),
                        SurName = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        SeniorCitizenId = c.Int(nullable: false),
                        PwdId = c.Int(nullable: false),
                        SoloParentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClientId)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .Index(t => t.CityId);
            
            AddColumn("dbo.Requirements", "ClientId", c => c.Int(nullable: false));
            AlterColumn("dbo.SeniorCitizens", "SeniorCitizenId", c => c.Int(nullable: false));
            AlterColumn("dbo.SeniorCitizens", "City_CityId", c => c.Int());
            AlterColumn("dbo.SoloParents", "SoloParentId", c => c.Int(nullable: false));
            AlterColumn("dbo.SoloParents", "City_CityId", c => c.Int());
            AlterColumn("dbo.Pwds", "PwdId", c => c.Int(nullable: false));
            AlterColumn("dbo.Pwds", "City_CityId", c => c.Int());
            AddPrimaryKey("dbo.SeniorCitizens", "SeniorCitizenId");
            AddPrimaryKey("dbo.SoloParents", "SoloParentId");
            AddPrimaryKey("dbo.Pwds", "PwdId");
            CreateIndex("dbo.Pwds", "PwdId");
            CreateIndex("dbo.Pwds", "City_CityId");
            CreateIndex("dbo.Requirements", "ClientId");
            CreateIndex("dbo.SeniorCitizens", "SeniorCitizenId");
            CreateIndex("dbo.SeniorCitizens", "City_CityId");
            CreateIndex("dbo.SoloParents", "SoloParentId");
            CreateIndex("dbo.SoloParents", "City_CityId");
            AddForeignKey("dbo.Pwds", "PwdId", "dbo.Clients", "ClientId");
            AddForeignKey("dbo.Requirements", "ClientId", "dbo.Clients", "ClientId", cascadeDelete: true);
            AddForeignKey("dbo.SeniorCitizens", "SeniorCitizenId", "dbo.Clients", "ClientId");
            AddForeignKey("dbo.SoloParents", "SoloParentId", "dbo.Clients", "ClientId");
            AddForeignKey("dbo.Pwds", "City_CityId", "dbo.Cities", "CityId");
            AddForeignKey("dbo.SeniorCitizens", "City_CityId", "dbo.Cities", "CityId");
            AddForeignKey("dbo.SoloParents", "City_CityId", "dbo.Cities", "CityId");
            DropColumn("dbo.Requirements", "PWD_PWDId");
            DropColumn("dbo.Requirements", "SeniorCitizen_SeniorCitizenId");
            DropColumn("dbo.Requirements", "SoloParent_SoloParentId");
            DropColumn("dbo.SeniorCitizens", "GivenName");
            DropColumn("dbo.SeniorCitizens", "MiddleName");
            DropColumn("dbo.SeniorCitizens", "SurName");
            DropColumn("dbo.SeniorCitizens", "DateCreated");
            DropColumn("dbo.SoloParents", "GivenName");
            DropColumn("dbo.SoloParents", "MiddleName");
            DropColumn("dbo.SoloParents", "SurName");
            DropColumn("dbo.SoloParents", "DateCreated");
            DropColumn("dbo.Pwds", "GivenName");
            DropColumn("dbo.Pwds", "MiddleName");
            DropColumn("dbo.Pwds", "SurName");
            DropColumn("dbo.Pwds", "DateCreated");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pwds", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Pwds", "SurName", c => c.String());
            AddColumn("dbo.Pwds", "MiddleName", c => c.String());
            AddColumn("dbo.Pwds", "GivenName", c => c.String());
            AddColumn("dbo.SoloParents", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.SoloParents", "SurName", c => c.String());
            AddColumn("dbo.SoloParents", "MiddleName", c => c.String());
            AddColumn("dbo.SoloParents", "GivenName", c => c.String());
            AddColumn("dbo.SeniorCitizens", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.SeniorCitizens", "SurName", c => c.String());
            AddColumn("dbo.SeniorCitizens", "MiddleName", c => c.String());
            AddColumn("dbo.SeniorCitizens", "GivenName", c => c.String());
            AddColumn("dbo.Requirements", "SoloParent_SoloParentId", c => c.Int());
            AddColumn("dbo.Requirements", "SeniorCitizen_SeniorCitizenId", c => c.Int());
            AddColumn("dbo.Requirements", "PWD_PWDId", c => c.Int());
            DropForeignKey("dbo.SoloParents", "City_CityId", "dbo.Cities");
            DropForeignKey("dbo.SeniorCitizens", "City_CityId", "dbo.Cities");
            DropForeignKey("dbo.Pwds", "City_CityId", "dbo.Cities");
            DropForeignKey("dbo.SoloParents", "SoloParentId", "dbo.Clients");
            DropForeignKey("dbo.SeniorCitizens", "SeniorCitizenId", "dbo.Clients");
            DropForeignKey("dbo.Requirements", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Pwds", "PwdId", "dbo.Clients");
            DropForeignKey("dbo.Clients", "CityId", "dbo.Cities");
            DropIndex("dbo.SoloParents", new[] { "City_CityId" });
            DropIndex("dbo.SoloParents", new[] { "SoloParentId" });
            DropIndex("dbo.SeniorCitizens", new[] { "City_CityId" });
            DropIndex("dbo.SeniorCitizens", new[] { "SeniorCitizenId" });
            DropIndex("dbo.Requirements", new[] { "ClientId" });
            DropIndex("dbo.Pwds", new[] { "City_CityId" });
            DropIndex("dbo.Pwds", new[] { "PwdId" });
            DropIndex("dbo.Clients", new[] { "CityId" });
            DropPrimaryKey("dbo.Pwds");
            DropPrimaryKey("dbo.SoloParents");
            DropPrimaryKey("dbo.SeniorCitizens");
            AlterColumn("dbo.Pwds", "City_CityId", c => c.Int(nullable: false));
            AlterColumn("dbo.Pwds", "PwdId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.SoloParents", "City_CityId", c => c.Int(nullable: false));
            AlterColumn("dbo.SoloParents", "SoloParentId", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.SeniorCitizens", "City_CityId", c => c.Int(nullable: false));
            AlterColumn("dbo.SeniorCitizens", "SeniorCitizenId", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Requirements", "ClientId");
            DropTable("dbo.Clients");
            AddPrimaryKey("dbo.Pwds", "PWDId");
            AddPrimaryKey("dbo.SoloParents", "SoloParentId");
            AddPrimaryKey("dbo.SeniorCitizens", "SeniorCitizenId");
            RenameColumn(table: "dbo.SoloParents", name: "City_CityId", newName: "CityId");
            RenameColumn(table: "dbo.SeniorCitizens", name: "City_CityId", newName: "CityId");
            RenameColumn(table: "dbo.Pwds", name: "City_CityId", newName: "CityId");
            CreateIndex("dbo.SoloParents", "CityId");
            CreateIndex("dbo.SeniorCitizens", "CityId");
            CreateIndex("dbo.Requirements", "SoloParent_SoloParentId");
            CreateIndex("dbo.Requirements", "SeniorCitizen_SeniorCitizenId");
            CreateIndex("dbo.Requirements", "PWD_PWDId");
            CreateIndex("dbo.Pwds", "CityId");
            AddForeignKey("dbo.SoloParents", "CityId", "dbo.Cities", "CityId", cascadeDelete: true);
            AddForeignKey("dbo.SeniorCitizens", "CityId", "dbo.Cities", "CityId", cascadeDelete: true);
            AddForeignKey("dbo.PWDs", "CityId", "dbo.Cities", "CityId", cascadeDelete: true);
            AddForeignKey("dbo.Requirements", "SoloParent_SoloParentId", "dbo.SoloParents", "SoloParentId");
            AddForeignKey("dbo.Requirements", "SeniorCitizen_SeniorCitizenId", "dbo.SeniorCitizens", "SeniorCitizenId");
            AddForeignKey("dbo.Requirements", "PWD_PWDId", "dbo.PWDs", "PWDId");
        }
    }
}
