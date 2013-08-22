namespace Vendord.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Vendord.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Vendord.DAL.VendordContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Vendord.DAL.VendordContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.

            context.Vendors.AddOrUpdate(
                v => v.Name,
                new Vendor { Name = "BigFont Farms" },
                new Vendor { Name = "BigFont Vineyards" },
                new Vendor { Name = "Nature Doesn't Work" },
                new Vendor { Name = "Carrots are Us" }
                );
        }
    }
}
