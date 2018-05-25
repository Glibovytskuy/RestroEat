namespace Restro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class username : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CommentModels", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CommentModels", "UserName");
        }
    }
}
