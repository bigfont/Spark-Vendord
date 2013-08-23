namespace Vendord.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vendorProductUnitPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VendorProducts", "UnitPrice", c => c.Decimal(nullable: false, storeType: "money"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VendorProducts", "UnitPrice");
        }
    }
}
