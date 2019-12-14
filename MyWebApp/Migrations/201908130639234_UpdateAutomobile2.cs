namespace MyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAutomobile2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Automobiles", "CarBrand", c => c.String(nullable: false));
            AlterColumn("dbo.Automobiles", "CarModel", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Automobiles", "CarModel", c => c.String());
            AlterColumn("dbo.Automobiles", "CarBrand", c => c.String());
        }
    }
}
