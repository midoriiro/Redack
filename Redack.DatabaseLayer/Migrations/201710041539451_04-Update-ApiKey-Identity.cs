namespace Redack.DatabaseLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _04UpdateApiKeyIdentity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApiKeys", "User_Id", "dbo.Users");
            DropIndex("dbo.ApiKeys", new[] { "User_Id" });
            AddColumn("dbo.Credentials", "ApiKey_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Credentials", "ApiKey_Id");
            AddForeignKey("dbo.Credentials", "ApiKey_Id", "dbo.ApiKeys", "Id", cascadeDelete: true);
            DropColumn("dbo.ApiKeys", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApiKeys", "User_Id", c => c.Int(nullable: false));
            DropForeignKey("dbo.Credentials", "ApiKey_Id", "dbo.ApiKeys");
            DropIndex("dbo.Credentials", new[] { "ApiKey_Id" });
            DropColumn("dbo.Credentials", "ApiKey_Id");
            CreateIndex("dbo.ApiKeys", "User_Id");
            AddForeignKey("dbo.ApiKeys", "User_Id", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
