namespace Redack.DatabaseLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _05UpdateApiKeyIdentity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApiKeys", "Key", c => c.String(nullable: false, maxLength: 255));
            CreateIndex("dbo.ApiKeys", "Key", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.ApiKeys", new[] { "Key" });
            DropColumn("dbo.ApiKeys", "Key");
        }
    }
}
