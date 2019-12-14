namespace MyWebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOffers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BeginDate = c.DateTime(nullable: false),
                        DateStop = c.DateTime(nullable: false),
                        PriceAtDay = c.Double(nullable: false),
                        AutomobileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Automobiles", t => t.AutomobileId, cascadeDelete: true)
                .Index(t => t.AutomobileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Offers", "AutomobileId", "dbo.Automobiles");
            DropIndex("dbo.Offers", new[] { "AutomobileId" });
            DropTable("dbo.Offers");
        }
    }
}
