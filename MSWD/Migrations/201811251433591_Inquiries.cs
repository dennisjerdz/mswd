namespace MSWD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inquiries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inquiries",
                c => new
                    {
                        InquiryId = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        MessageId = c.Int(),
                        Status = c.String(),
                        Category = c.String(),
                        Content = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.InquiryId)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("dbo.Messages", t => t.MessageId)
                .Index(t => t.ClientId)
                .Index(t => t.MessageId);
            
            CreateTable(
                "dbo.InquiryReplies",
                c => new
                    {
                        InquiryReplyId = c.Int(nullable: false, identity: true),
                        InquiryId = c.Int(nullable: false),
                        UserName = c.String(),
                        Content = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.InquiryReplyId)
                .ForeignKey("dbo.Inquiries", t => t.InquiryId, cascadeDelete: true)
                .Index(t => t.InquiryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inquiries", "MessageId", "dbo.Messages");
            DropForeignKey("dbo.InquiryReplies", "InquiryId", "dbo.Inquiries");
            DropForeignKey("dbo.Inquiries", "ClientId", "dbo.Clients");
            DropIndex("dbo.InquiryReplies", new[] { "InquiryId" });
            DropIndex("dbo.Inquiries", new[] { "MessageId" });
            DropIndex("dbo.Inquiries", new[] { "ClientId" });
            DropTable("dbo.InquiryReplies");
            DropTable("dbo.Inquiries");
        }
    }
}
