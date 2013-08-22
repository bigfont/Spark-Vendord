namespace Vendord.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vendorProductModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VendorProducts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        VendorID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Vendors", t => t.VendorID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.VendorID)
                .Index(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.VendorProducts", new[] { "ProductID" });
            DropIndex("dbo.VendorProducts", new[] { "VendorID" });
            DropForeignKey("dbo.VendorProducts", "ProductID", "dbo.Products");
            DropForeignKey("dbo.VendorProducts", "VendorID", "dbo.Vendors");
            DropTable("dbo.VendorProducts");
        }
    }
}
