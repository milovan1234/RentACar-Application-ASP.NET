namespace MyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAutomobile4 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CardBodies", newName: "CarBodies");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.CarBodies", newName: "CardBodies");
        }
    }
}
