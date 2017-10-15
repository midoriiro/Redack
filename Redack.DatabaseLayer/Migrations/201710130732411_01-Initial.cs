namespace Redack.DatabaseLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _01Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApiKeys",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Key = c.String(nullable: false, maxLength: 255),
                        Client_Id = c.Int(),
                        Credential_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id, cascadeDelete: true)
                .ForeignKey("dbo.Credentials", t => t.Credential_Id, cascadeDelete: true)
                .Index(t => t.Key, unique: true)
                .Index(t => t.Client_Id)
                .Index(t => t.Credential_Id);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        IsBlocked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Credentials",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Login = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false),
                        Salt = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.Login, unique: true);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 15),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Codename = c.String(nullable: false),
                        HelpText = c.String(nullable: false),
                        ContentType = c.String(nullable: false),
                        Group_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.Group_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Group_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Alias = c.String(nullable: false, maxLength: 15),
                        IdentIcon = c.String(),
                        Group_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.Group_Id)
                .Index(t => t.Alias)
                .Index(t => t.Group_Id);
            
            CreateTable(
                "dbo.Identities",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Access = c.String(nullable: false),
                        Refresh = c.String(nullable: false),
                        Client_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clients", t => t.Client_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Client_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        Text = c.String(nullable: false),
                        CreatedBy_Id = c.Int(),
                        Thread_Id = c.Int(nullable: false),
                        UpdatedBy_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedBy_Id)
                .ForeignKey("dbo.Threads", t => t.Thread_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UpdatedBy_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.Thread_Id)
                .Index(t => t.UpdatedBy_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Threads",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 50),
                        Node_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Nodes", t => t.Node_Id, cascadeDelete: true)
                .Index(t => t.Title)
                .Index(t => t.Description)
                .Index(t => t.Node_Id);
            
            CreateTable(
                "dbo.Nodes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Permissions", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Messages", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Messages", "UpdatedBy_Id", "dbo.Users");
            DropForeignKey("dbo.Messages", "Thread_Id", "dbo.Threads");
            DropForeignKey("dbo.Threads", "Node_Id", "dbo.Nodes");
            DropForeignKey("dbo.Messages", "CreatedBy_Id", "dbo.Users");
            DropForeignKey("dbo.Identities", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Identities", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.Users", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.Credentials", "Id", "dbo.Users");
            DropForeignKey("dbo.Permissions", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.ApiKeys", "Credential_Id", "dbo.Credentials");
            DropForeignKey("dbo.ApiKeys", "Client_Id", "dbo.Clients");
            DropIndex("dbo.Nodes", new[] { "Name" });
            DropIndex("dbo.Threads", new[] { "Node_Id" });
            DropIndex("dbo.Threads", new[] { "Description" });
            DropIndex("dbo.Threads", new[] { "Title" });
            DropIndex("dbo.Messages", new[] { "User_Id" });
            DropIndex("dbo.Messages", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.Messages", new[] { "Thread_Id" });
            DropIndex("dbo.Messages", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Identities", new[] { "User_Id" });
            DropIndex("dbo.Identities", new[] { "Client_Id" });
            DropIndex("dbo.Users", new[] { "Group_Id" });
            DropIndex("dbo.Users", new[] { "Alias" });
            DropIndex("dbo.Permissions", new[] { "User_Id" });
            DropIndex("dbo.Permissions", new[] { "Group_Id" });
            DropIndex("dbo.Groups", new[] { "Name" });
            DropIndex("dbo.Credentials", new[] { "Login" });
            DropIndex("dbo.Credentials", new[] { "Id" });
            DropIndex("dbo.ApiKeys", new[] { "Credential_Id" });
            DropIndex("dbo.ApiKeys", new[] { "Client_Id" });
            DropIndex("dbo.ApiKeys", new[] { "Key" });
            DropTable("dbo.Nodes");
            DropTable("dbo.Threads");
            DropTable("dbo.Messages");
            DropTable("dbo.Identities");
            DropTable("dbo.Users");
            DropTable("dbo.Permissions");
            DropTable("dbo.Groups");
            DropTable("dbo.Credentials");
            DropTable("dbo.Clients");
            DropTable("dbo.ApiKeys");
        }
    }
}
