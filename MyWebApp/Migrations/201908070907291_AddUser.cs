namespace MyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "FirstName", c => c.String(nullable: false));
            AlterColumn("dbo.Users", "LastName", c => c.String(nullable: false));
            DropColumn("dbo.Users", "FisrstName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "FisrstName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Users", "LastName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Users", "FirstName");
        }
    }
}
