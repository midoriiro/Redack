namespace Redack.DatabaseLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _02addconstrainttouseridenticon : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "IdentIcon", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "IdentIcon", c => c.String());
        }
    }
}
