namespace MyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReservationTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AutomobileId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        BeginDate = c.DateTime(nullable: false),
                        DateStop = c.DateTime(nullable: false),
                        totalPrice = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Automobiles", t => t.AutomobileId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.AutomobileId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "UserId", "dbo.Users");
            DropForeignKey("dbo.Reservations", "AutomobileId", "dbo.Automobiles");
            DropIndex("dbo.Reservations", new[] { "UserId" });
            DropIndex("dbo.Reservations", new[] { "AutomobileId" });
            DropTable("dbo.Reservations");
        }
    }
}
