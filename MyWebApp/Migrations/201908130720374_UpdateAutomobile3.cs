namespace MyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAutomobile3 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Automobiles", name: "CardBodyId", newName: "CarBodyId");
            RenameIndex(table: "dbo.Automobiles", name: "IX_CardBodyId", newName: "IX_CarBodyId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Automobiles", name: "IX_CarBodyId", newName: "IX_CardBodyId");
            RenameColumn(table: "dbo.Automobiles", name: "CarBodyId", newName: "CardBodyId");
        }
    }
}
