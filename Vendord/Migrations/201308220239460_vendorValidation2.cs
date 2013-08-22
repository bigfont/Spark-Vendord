namespace Vendord.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vendorValidation2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Vendors", "Name", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vendors", "Name", c => c.String(nullable: false));
        }
    }
}
