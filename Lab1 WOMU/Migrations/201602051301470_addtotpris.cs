namespace Lab1_WOMU.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtotpris : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CartItems", "totPris", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CartItems", "totPris");
        }
    }
}
