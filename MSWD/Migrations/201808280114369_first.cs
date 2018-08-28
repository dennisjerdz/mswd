namespace MSWD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        CityId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CityId);
            
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
            
            CreateTable(
                "dbo.Pwds",
                c => new
                    {
                        PwdId = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PwdId)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.Requirements",
                c => new
                    {
                        RequirementId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        IsDone = c.Boolean(nullable: false),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RequirementId)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.RequirementAttachments",
                c => new
                    {
                        RequirementAttachmentId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Location = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        RequirementId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RequirementAttachmentId)
                .ForeignKey("dbo.Requirements", t => t.RequirementId, cascadeDelete: true)
                .Index(t => t.RequirementId);
            
            CreateTable(
                "dbo.SeniorCitizens",
                c => new
                    {
                        SeniorCitizenId = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SeniorCitizenId)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.SoloParents",
                c => new
                    {
                        SoloParentId = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SoloParentId)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.SoloParents", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.SeniorCitizens", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Requirements", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.RequirementAttachments", "RequirementId", "dbo.Requirements");
            DropForeignKey("dbo.Pwds", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Clients", "CityId", "dbo.Cities");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.SoloParents", new[] { "ClientId" });
            DropIndex("dbo.SeniorCitizens", new[] { "ClientId" });
            DropIndex("dbo.RequirementAttachments", new[] { "RequirementId" });
            DropIndex("dbo.Requirements", new[] { "ClientId" });
            DropIndex("dbo.Pwds", new[] { "ClientId" });
            DropIndex("dbo.Clients", new[] { "CityId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.SoloParents");
            DropTable("dbo.SeniorCitizens");
            DropTable("dbo.RequirementAttachments");
            DropTable("dbo.Requirements");
            DropTable("dbo.Pwds");
            DropTable("dbo.Clients");
            DropTable("dbo.Cities");
        }
    }
}
