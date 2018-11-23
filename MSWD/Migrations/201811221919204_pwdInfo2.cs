namespace MSWD.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pwdInfo2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "EmploymentStatus", c => c.String());
            AddColumn("dbo.Clients", "Position", c => c.String());
            AddColumn("dbo.Clients", "Company", c => c.String());
            AddColumn("dbo.Clients", "NatureOfEmployer", c => c.String());
            AddColumn("dbo.Clients", "TypeOfEmployment", c => c.String());
            AddColumn("dbo.Clients", "SSSNo", c => c.String());
            AddColumn("dbo.Clients", "GSISNo", c => c.String());
            AddColumn("dbo.Clients", "PhilhealthNo", c => c.String());
            AddColumn("dbo.Clients", "PhilHealthMembershipType", c => c.String());
            AddColumn("dbo.Clients", "YellowCardNo", c => c.String());
            AddColumn("dbo.Clients", "YellowCardMembershipType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Clients", "YellowCardMembershipType");
            DropColumn("dbo.Clients", "YellowCardNo");
            DropColumn("dbo.Clients", "PhilHealthMembershipType");
            DropColumn("dbo.Clients", "PhilhealthNo");
            DropColumn("dbo.Clients", "GSISNo");
            DropColumn("dbo.Clients", "SSSNo");
            DropColumn("dbo.Clients", "TypeOfEmployment");
            DropColumn("dbo.Clients", "NatureOfEmployer");
            DropColumn("dbo.Clients", "Company");
            DropColumn("dbo.Clients", "Position");
            DropColumn("dbo.Clients", "EmploymentStatus");
        }
    }
}
