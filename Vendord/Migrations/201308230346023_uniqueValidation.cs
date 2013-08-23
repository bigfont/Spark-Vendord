namespace Vendord.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uniqueValidation : DbMigration
    {
        public override void Up()
        {
            string template = "ALTER TABLE {0} ADD CONSTRAINT {1} UNIQUE NONCLUSTERED ({2})";

            Sql(String.Format(template, "Vendors", "UQ_Vendors_Name", "Name"));
            Sql(String.Format(template, "Products", "UQ_Products_Name", "Name"));
            Sql(String.Format(template, "VendorProducts", "UQ_VendorProducts_VendorID_ProductID", "VendorID, ProductID"));

        }

        public override void Down()
        {
            string template = "ALTER TABLE {0} DROP CONSTRAINT {1}";
            Sql(String.Format(template, "Vendors", "UQ_Vendors_Name"));
            Sql(String.Format(template, "Products", "UQ_Products_Name"));
            Sql(String.Format(template, "VendorProducts", "UQ_VendorProducts_VendorID_ProductID"));

        }
    }
}
