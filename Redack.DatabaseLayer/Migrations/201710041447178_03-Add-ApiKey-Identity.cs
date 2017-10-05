namespace Redack.DatabaseLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _03AddApiKeyIdentity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "Thread_Id", "dbo.Threads");
            DropForeignKey("dbo.Threads", "Node_Id", "dbo.Nodes");
            DropIndex("dbo.Groups", new[] { "Name" });
            DropIndex("dbo.Messages", new[] { "Thread_Id" });
            DropIndex("dbo.Threads", new[] { "Title" });
            DropIndex("dbo.Threads", new[] { "Description" });
            DropIndex("dbo.Threads", new[] { "Node_Id" });
            CreateTable(
                "dbo.ApiKeys",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Identities",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Token = c.String(nullable: false),
                        ApiKey_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApiKeys", t => t.ApiKey_Id, cascadeDelete: true)
                .Index(t => t.ApiKey_Id);
            
            AlterColumn("dbo.Groups", "Name", c => c.String(nullable: false, maxLength: 15));
            AlterColumn("dbo.Messages", "Thread_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Threads", "Title", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Threads", "Description", c => c.String(maxLength: 50));
            AlterColumn("dbo.Threads", "Node_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Groups", "Name", unique: true);
            CreateIndex("dbo.Messages", "Thread_Id");
            CreateIndex("dbo.Threads", "Title");
            CreateIndex("dbo.Threads", "Description");
            CreateIndex("dbo.Threads", "Node_Id");
            AddForeignKey("dbo.Messages", "Thread_Id", "dbo.Threads", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Threads", "Node_Id", "dbo.Nodes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Threads", "Node_Id", "dbo.Nodes");
            DropForeignKey("dbo.Messages", "Thread_Id", "dbo.Threads");
            DropForeignKey("dbo.Identities", "ApiKey_Id", "dbo.ApiKeys");
            DropForeignKey("dbo.ApiKeys", "User_Id", "dbo.Users");
            DropIndex("dbo.Identities", new[] { "ApiKey_Id" });
            DropIndex("dbo.Threads", new[] { "Node_Id" });
            DropIndex("dbo.Threads", new[] { "Description" });
            DropIndex("dbo.Threads", new[] { "Title" });
            DropIndex("dbo.Messages", new[] { "Thread_Id" });
            DropIndex("dbo.Groups", new[] { "Name" });
            DropIndex("dbo.ApiKeys", new[] { "User_Id" });
            AlterColumn("dbo.Threads", "Node_Id", c => c.Int());
            AlterColumn("dbo.Threads", "Description", c => c.String(maxLength: 100));
            AlterColumn("dbo.Threads", "Title", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Messages", "Thread_Id", c => c.Int());
            AlterColumn("dbo.Groups", "Name", c => c.String(nullable: false, maxLength: 20));
            DropTable("dbo.Identities");
            DropTable("dbo.ApiKeys");
            CreateIndex("dbo.Threads", "Node_Id");
            CreateIndex("dbo.Threads", "Description");
            CreateIndex("dbo.Threads", "Title");
            CreateIndex("dbo.Messages", "Thread_Id");
            CreateIndex("dbo.Groups", "Name", unique: true);
            AddForeignKey("dbo.Threads", "Node_Id", "dbo.Nodes", "Id");
            AddForeignKey("dbo.Messages", "Thread_Id", "dbo.Threads", "Id");
        }
    }
}
