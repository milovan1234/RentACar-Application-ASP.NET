namespace MyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAutomobile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Automobiles", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Automobiles", "ImagePath");
        }
    }
}
