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
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.ContentType, t.Codename }, unique: true, name: "UIX_ContentTypeAndCodename");
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Alias = c.String(nullable: false, maxLength: 15),
                        IdentIcon = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Alias);
            
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
                        Date = c.DateTime(nullable: false),
                        Text = c.String(nullable: false),
                        Thread_Id = c.Int(nullable: false),
                        Author_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Threads", t => t.Thread_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.Author_Id)
                .Index(t => t.Thread_Id)
                .Index(t => t.Author_Id);
            
            CreateTable(
                "dbo.MessageHistories",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Editor_Id = c.Int(nullable: false),
                        Message_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Editor_Id, cascadeDelete: true)
                .ForeignKey("dbo.Messages", t => t.Message_Id)
                .Index(t => t.Editor_Id)
                .Index(t => t.Message_Id);
            
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
            
            CreateTable(
                "dbo.UserPermissions",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Permission_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Permission_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Permissions", t => t.Permission_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Permission_Id);
            
            CreateTable(
                "dbo.GroupPermissions",
                c => new
                    {
                        Group_Id = c.Int(nullable: false),
                        Permission_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Group_Id, t.Permission_Id })
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .ForeignKey("dbo.Permissions", t => t.Permission_Id, cascadeDelete: true)
                .Index(t => t.Group_Id)
                .Index(t => t.Permission_Id);
            
            CreateTable(
                "dbo.GroupUsers",
                c => new
                    {
                        Group_Id = c.Int(nullable: false),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Group_Id, t.User_Id })
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.Group_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroupUsers", "User_Id", "dbo.Users");
            DropForeignKey("dbo.GroupUsers", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.GroupPermissions", "Permission_Id", "dbo.Permissions");
            DropForeignKey("dbo.GroupPermissions", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.UserPermissions", "Permission_Id", "dbo.Permissions");
            DropForeignKey("dbo.UserPermissions", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Messages", "Author_Id", "dbo.Users");
            DropForeignKey("dbo.Messages", "Thread_Id", "dbo.Threads");
            DropForeignKey("dbo.Threads", "Node_Id", "dbo.Nodes");
            DropForeignKey("dbo.MessageHistories", "Message_Id", "dbo.Messages");
            DropForeignKey("dbo.MessageHistories", "Editor_Id", "dbo.Users");
            DropForeignKey("dbo.Identities", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Identities", "Client_Id", "dbo.Clients");
            DropForeignKey("dbo.Credentials", "Id", "dbo.Users");
            DropForeignKey("dbo.ApiKeys", "Credential_Id", "dbo.Credentials");
            DropForeignKey("dbo.ApiKeys", "Client_Id", "dbo.Clients");
            DropIndex("dbo.GroupUsers", new[] { "User_Id" });
            DropIndex("dbo.GroupUsers", new[] { "Group_Id" });
            DropIndex("dbo.GroupPermissions", new[] { "Permission_Id" });
            DropIndex("dbo.GroupPermissions", new[] { "Group_Id" });
            DropIndex("dbo.UserPermissions", new[] { "Permission_Id" });
            DropIndex("dbo.UserPermissions", new[] { "User_Id" });
            DropIndex("dbo.Nodes", new[] { "Name" });
            DropIndex("dbo.Threads", new[] { "Node_Id" });
            DropIndex("dbo.Threads", new[] { "Description" });
            DropIndex("dbo.Threads", new[] { "Title" });
            DropIndex("dbo.MessageHistories", new[] { "Message_Id" });
            DropIndex("dbo.MessageHistories", new[] { "Editor_Id" });
            DropIndex("dbo.Messages", new[] { "Author_Id" });
            DropIndex("dbo.Messages", new[] { "Thread_Id" });
            DropIndex("dbo.Identities", new[] { "User_Id" });
            DropIndex("dbo.Identities", new[] { "Client_Id" });
            DropIndex("dbo.Users", new[] { "Alias" });
            DropIndex("dbo.Permissions", "UIX_ContentTypeAndCodename");
            DropIndex("dbo.Groups", new[] { "Name" });
            DropIndex("dbo.Credentials", new[] { "Login" });
            DropIndex("dbo.Credentials", new[] { "Id" });
            DropIndex("dbo.ApiKeys", new[] { "Credential_Id" });
            DropIndex("dbo.ApiKeys", new[] { "Client_Id" });
            DropIndex("dbo.ApiKeys", new[] { "Key" });
            DropTable("dbo.GroupUsers");
            DropTable("dbo.GroupPermissions");
            DropTable("dbo.UserPermissions");
            DropTable("dbo.Nodes");
            DropTable("dbo.Threads");
            DropTable("dbo.MessageHistories");
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
