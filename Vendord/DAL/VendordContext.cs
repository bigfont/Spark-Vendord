using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Vendord.Models;

namespace Vendord.DAL
{
    public class VendordContext : DbContext
    {
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<VendorProduct> VendorProducts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();

            FluentApiHelper fluentHelper = new FluentApiHelper(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private class FluentApiHelper
        {
            private DbModelBuilder modelBuilder;

            public FluentApiHelper(DbModelBuilder modelBuilder)
            {
                this.modelBuilder = modelBuilder;
                ConfigureProduct();
                ConifigureVendor();
                ConfigureVendorProduct();
            }

            private void ConfigureProduct()
            {
                var entity = modelBuilder.Entity<Product>();
                entity.HasKey(e => e.ID);
                entity.Property(e => e.TimeStamp).IsRowVersion();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            }

            private void ConifigureVendor()
            {
                var entity = modelBuilder.Entity<Vendor>();
                entity.HasKey(e => e.ID);
                entity.Property(e => e.TimeStamp).IsRowVersion();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            }

            private void ConfigureVendorProduct()
            {
                var entity = modelBuilder.Entity<VendorProduct>();
                entity.HasKey(e => e.ID);
                entity.Property(e => e.UnitPrice).HasColumnType("money");
                entity.Property(e => e.Timestamp).IsRowVersion();
                entity.HasRequired(e => e.Vendor);
                entity.HasRequired(e => e.Product);                
            }
        }
    }
}