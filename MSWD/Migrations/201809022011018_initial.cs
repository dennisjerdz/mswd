namespace MSWD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
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
                        GivenName = c.String(nullable: false),
                        MiddleName = c.String(),
                        SurName = c.String(nullable: false),
                        Gender = c.String(nullable: false),
                        CivilStatus = c.String(nullable: false),
                        Occupation = c.String(),
                        Citizenship = c.String(),
                        CityId = c.Int(nullable: false),
                        CityAddress = c.String(nullable: false),
                        ProvincialAddress = c.String(),
                        ContactNumber = c.String(),
                        TypeOfResidency = c.String(),
                        StartOfResidency = c.DateTime(),
                        BirthDate = c.DateTime(nullable: false),
                        BirthPlace = c.String(nullable: false),
                        Religion = c.String(),
                        DateOfMarriage = c.DateTime(),
                        PlaceOfMarriage = c.String(),
                        SpouseName = c.String(),
                        SpouseBirthDate = c.DateTime(),
                        SpouseBluCardNo = c.String(),
                        CreatedByUserId = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ClientId)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedByUserId)
                .Index(t => t.CityId)
                .Index(t => t.CreatedByUserId);
            
            CreateTable(
                "dbo.ClientBeneficiaries",
                c => new
                    {
                        ClientBeneficiaryId = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        Name = c.String(),
                        Relationship = c.String(),
                        BirthDate = c.DateTime(),
                        ContactNumber = c.String(),
                        CivilStatus = c.String(),
                        Occupation = c.String(),
                        Income = c.String(),
                    })
                .PrimaryKey(t => t.ClientBeneficiaryId)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.ClientNotes",
                c => new
                    {
                        ClientNoteId = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        Note = c.String(nullable: false),
                        Done = c.Int(nullable: false),
                        CreatedByUserId = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ClientNoteId)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedByUserId)
                .Index(t => t.ClientId)
                .Index(t => t.CreatedByUserId);
            
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
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Pwds",
                c => new
                    {
                        PwdId = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        CreatedByUserId = c.String(maxLength: 128),
                        VerifiedByUserId = c.String(maxLength: 128),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PwdId)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedByUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.VerifiedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.VerifiedByUserId)
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
                        Status = c.String(),
                        CreatedByUserId = c.String(maxLength: 128),
                        VerifiedByUserId = c.String(maxLength: 128),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SeniorCitizenId)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedByUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.VerifiedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.VerifiedByUserId)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.SoloParents",
                c => new
                    {
                        SoloParentId = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        CreatedByUserId = c.String(maxLength: 128),
                        VerifiedByUserId = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SoloParentId)
                .ForeignKey("dbo.Clients", t => t.ClientId)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedByUserId)
                .ForeignKey("dbo.AspNetUsers", t => t.VerifiedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.VerifiedByUserId)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        MobileNumberId = c.Int(),
                        Body = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.MobileNumbers", t => t.MobileNumberId)
                .Index(t => t.MobileNumberId);
            
            CreateTable(
                "dbo.MobileNumbers",
                c => new
                    {
                        MobileNumberId = c.Int(nullable: false, identity: true),
                        MobileNo = c.String(nullable: false),
                        Token = c.String(),
                        IsDisabled = c.Boolean(nullable: false),
                        ClientId = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MobileNumberId)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Messages", "MobileNumberId", "dbo.MobileNumbers");
            DropForeignKey("dbo.MobileNumbers", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.SoloParents", "VerifiedByUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SoloParents", "CreatedByUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SoloParents", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.SeniorCitizens", "VerifiedByUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SeniorCitizens", "CreatedByUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SeniorCitizens", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Requirements", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.RequirementAttachments", "RequirementId", "dbo.Requirements");
            DropForeignKey("dbo.Pwds", "VerifiedByUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Pwds", "CreatedByUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Pwds", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Clients", "CreatedByUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ClientNotes", "CreatedByUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ClientNotes", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.ClientBeneficiaries", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Clients", "CityId", "dbo.Cities");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.MobileNumbers", new[] { "ClientId" });
            DropIndex("dbo.Messages", new[] { "MobileNumberId" });
            DropIndex("dbo.SoloParents", new[] { "ClientId" });
            DropIndex("dbo.SoloParents", new[] { "VerifiedByUserId" });
            DropIndex("dbo.SoloParents", new[] { "CreatedByUserId" });
            DropIndex("dbo.SeniorCitizens", new[] { "ClientId" });
            DropIndex("dbo.SeniorCitizens", new[] { "VerifiedByUserId" });
            DropIndex("dbo.SeniorCitizens", new[] { "CreatedByUserId" });
            DropIndex("dbo.RequirementAttachments", new[] { "RequirementId" });
            DropIndex("dbo.Requirements", new[] { "ClientId" });
            DropIndex("dbo.Pwds", new[] { "ClientId" });
            DropIndex("dbo.Pwds", new[] { "VerifiedByUserId" });
            DropIndex("dbo.Pwds", new[] { "CreatedByUserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.ClientNotes", new[] { "CreatedByUserId" });
            DropIndex("dbo.ClientNotes", new[] { "ClientId" });
            DropIndex("dbo.ClientBeneficiaries", new[] { "ClientId" });
            DropIndex("dbo.Clients", new[] { "CreatedByUserId" });
            DropIndex("dbo.Clients", new[] { "CityId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.MobileNumbers");
            DropTable("dbo.Messages");
            DropTable("dbo.SoloParents");
            DropTable("dbo.SeniorCitizens");
            DropTable("dbo.RequirementAttachments");
            DropTable("dbo.Requirements");
            DropTable("dbo.Pwds");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ClientNotes");
            DropTable("dbo.ClientBeneficiaries");
            DropTable("dbo.Clients");
            DropTable("dbo.Cities");
        }
    }
}
