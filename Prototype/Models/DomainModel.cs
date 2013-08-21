﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Prototype.Models
{
    public class Vendor
    {
        public int VendorID { get; set; }
        public string VendorName { get; set; }
        public virtual List<Product> Products { get; set; }

        public Vendor()
        {
            Products = new List<Product>();
        }
    }
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductUpc { get; set; }
        public int VendorID { get; set; }
        public virtual Vendor Vendor { get; set; }
    }
    public class VendordContext : DbContext
    {
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Product> Products { get; set; }

        public VendordContext()
            : base("VendordSQLExpress")
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //do advanced migrations here

            base.OnModelCreating(modelBuilder);
        }
    }
}