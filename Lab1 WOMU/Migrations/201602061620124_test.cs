namespace Lab1_WOMU.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
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
                        totPris = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CartItemID)
                .ForeignKey("dbo.Produkts", t => t.ProduktID, cascadeDelete: true)
                .Index(t => t.ProduktID);
            
            CreateTable(
                "dbo.Produkts",
                c => new
                    {
                        ProduktID = c.Int(nullable: false, identity: true),
                        ProduktNamn = c.String(nullable: false),
                        ProduktBeskrivning = c.String(nullable: false),
                        Pris = c.Int(nullable: false),
                        AntalILager = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProduktID);
            
            CreateTable(
                "dbo.Kunds",
                c => new
                    {
                        KundID = c.Int(nullable: false, identity: true),
                        FörNamn = c.String(nullable: false),
                        Efternamn = c.String(nullable: false),
                        PostAdress = c.String(nullable: false),
                        PostNr = c.Int(nullable: false),
                        Ort = c.String(nullable: false),
                        Epost = c.String(nullable: false),
                        Telefonnummer = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.KundID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false),
                        Total = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID);
            
            CreateTable(
                "dbo.OrderRads",
                c => new
                    {
                        ProduktID = c.Int(nullable: false),
                        OrderID = c.Int(nullable: false),
                        TotalPris = c.Int(nullable: false),
                        Antal = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProduktID, t.OrderID })
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.Produkts", t => t.ProduktID, cascadeDelete: true)
                .Index(t => t.ProduktID)
                .Index(t => t.OrderID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderRads", "ProduktID", "dbo.Produkts");
            DropForeignKey("dbo.OrderRads", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.CartItems", "ProduktID", "dbo.Produkts");
            DropIndex("dbo.OrderRads", new[] { "OrderID" });
            DropIndex("dbo.OrderRads", new[] { "ProduktID" });
            DropIndex("dbo.CartItems", new[] { "ProduktID" });
            DropTable("dbo.OrderRads");
            DropTable("dbo.Orders");
            DropTable("dbo.Kunds");
            DropTable("dbo.Produkts");
            DropTable("dbo.CartItems");
        }
    }
}
