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
                new Vendor { Name = "Carrots are Us" },
                new Vendor { Name = "We Love Lettuce" }
            };

            foreach (Vendor v in vendors)
            {
                var exists = context.Vendors.Where(p => p.Name == v.Name).FirstOrDefault();
                if (exists == null)
                {
                    context.Vendors.Add(v);
                }
            }
            context.SaveChanges();

            var products = new List<Product>
            {
                new Product { Name = "Yams" },
                new Product { Name = "Oranges" },
                new Product { Name = "Apples" },
                new Product { Name = "Grapefruits" },
                new Product { Name = "Bananas" }
            };

            foreach (Product p in products)
            {
                var exists = context.Products.Where(pred => pred.Name == p.Name).FirstOrDefault();
                if (exists == null)
                {
                    context.Products.Add(p);
                }
            }
            context.SaveChanges();

            var vendorProducts = new List<VendorProduct>();
            decimal unitPrice = 1.00m;

            vendors = context.Vendors.ToList();
            products = context.Products.ToList();

            foreach (Vendor v in vendors)
            {
                foreach (Product p in products)
                {
                    var exists = context.VendorProducts.Where(pred => pred.ProductID == p.ID && pred.VendorID == v.ID).FirstOrDefault();
                    if (exists == null)
                    {
                        context.VendorProducts.Add(new VendorProduct { VendorID = v.ID, ProductID = p.ID, UnitPrice = unitPrice });
                    }
                    unitPrice += 0.25m;
                }
            }

            context.SaveChanges();

        }
    }
}
