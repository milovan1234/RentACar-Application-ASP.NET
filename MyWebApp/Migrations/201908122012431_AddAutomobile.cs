namespace MyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAutomobile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Automobiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CarBrand = c.String(),
                        CarModel = c.String(),
                        ProductYear = c.Int(nullable: false),
                        Cubicase = c.Int(nullable: false),
                        NumberOfDoorId = c.Int(nullable: false),
                        CardBodyId = c.Int(nullable: false),
                        GearshiftId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CardBodies", t => t.CardBodyId, cascadeDelete: true)
                .ForeignKey("dbo.Gearshifts", t => t.GearshiftId, cascadeDelete: true)
                .ForeignKey("dbo.NumberOfDoors", t => t.NumberOfDoorId, cascadeDelete: true)
                .Index(t => t.NumberOfDoorId)
                .Index(t => t.CardBodyId)
                .Index(t => t.GearshiftId);
            
            CreateTable(
                "dbo.CardBodies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Gearshifts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Shift = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NumberOfDoors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NumberDoor = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Automobiles", "NumberOfDoorId", "dbo.NumberOfDoors");
            DropForeignKey("dbo.Automobiles", "GearshiftId", "dbo.Gearshifts");
            DropForeignKey("dbo.Automobiles", "CardBodyId", "dbo.CardBodies");
            DropIndex("dbo.Automobiles", new[] { "GearshiftId" });
            DropIndex("dbo.Automobiles", new[] { "CardBodyId" });
            DropIndex("dbo.Automobiles", new[] { "NumberOfDoorId" });
            DropTable("dbo.NumberOfDoors");
            DropTable("dbo.Gearshifts");
            DropTable("dbo.CardBodies");
            DropTable("dbo.Automobiles");
        }
    }
}
