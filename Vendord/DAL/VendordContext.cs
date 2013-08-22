using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Vendord.Models;

namespace Vendord.DAL
{
    public class VendordContext : DbContext
    {
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}