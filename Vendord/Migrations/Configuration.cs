namespace Vendord.Migrations
{
    using System;
    using System.Collections.Generic;
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

            var vendors = new List<Vendor>
            {
                new Vendor { Name = "BigFont Farms" },
                new Vendor { Name = "BigFont Vineyards" },
                new Vendor { Name = "Nature Doesn't Work" },
                new Vendor { Name = "Carrots are Us" }
            };

            vendors.ForEach(v => context.Vendors.AddOrUpdate(cols => cols.Name, v));
            context.SaveChanges();

            var products = new List<Product>
            {
                new Product { Name = "Yams" },
                new Product { Name = "Oranges" },
                new Product { Name = "Apples" },
                new Product { Name = "Grapefruits" },
                new Product { Name = "Bananas" }
            };

            products.ForEach(p => context.Products.AddOrUpdate(cols => cols.Name, p));
            context.SaveChanges();

            var vendorProducts = new List<VendorProduct>();
            decimal unitPrice = 1.00m;

            foreach (Vendor v in vendors)
            {
                foreach (Product p in products)
                {
                    vendorProducts.Add(new VendorProduct
                    {
                        VendorID = v.ID,
                        ProductID = p.ID,
                        UnitPrice = unitPrice
                    });
                    unitPrice += 0.25m;
                }
            }

            vendorProducts.ForEach(vp => context.VendorProducts.AddOrUpdate(cols => new { cols.VendorID, cols.ProductID }, vp));
            context.SaveChanges();

        }
    }
}
