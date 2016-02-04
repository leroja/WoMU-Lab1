namespace Lab1_WOMU.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCartItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartItems",
                c => new
                    {
                        CartItemID = c.Int(nullable: false, identity: true),
                        CartId = c.String(),
                        ProduktID = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CartItemID)
                .ForeignKey("dbo.Produkts", t => t.ProduktID, cascadeDelete: true)
                .Index(t => t.ProduktID);
            
            AddColumn("dbo.Orders", "Total", c => c.Int(nullable: false));
            AddColumn("dbo.OrderRads", "TotalPris", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CartItems", "ProduktID", "dbo.Produkts");
            DropIndex("dbo.CartItems", new[] { "ProduktID" });
            DropColumn("dbo.OrderRads", "TotalPris");
            DropColumn("dbo.Orders", "Total");
            DropTable("dbo.CartItems");
        }
    }
}
