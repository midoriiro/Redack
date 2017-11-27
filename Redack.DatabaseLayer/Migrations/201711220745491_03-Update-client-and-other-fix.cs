namespace Redack.DatabaseLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _03Updateclientandotherfix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "PassPhrase", c => c.String(nullable: false));
            AddColumn("dbo.Clients", "Salt", c => c.String(nullable: false));
            AddColumn("dbo.Users", "IsAdministrator", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "IsAdministrator");
            DropColumn("dbo.Clients", "Salt");
            DropColumn("dbo.Clients", "PassPhrase");
        }
    }
}
