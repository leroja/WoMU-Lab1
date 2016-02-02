namespace Lab1_WOMU.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrFKPro : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.OrderRads", "ProduktID");
            AddForeignKey("dbo.OrderRads", "ProduktID", "dbo.Produkts", "ProduktID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderRads", "ProduktID", "dbo.Produkts");
            DropIndex("dbo.OrderRads", new[] { "ProduktID" });
        }
    }
}
