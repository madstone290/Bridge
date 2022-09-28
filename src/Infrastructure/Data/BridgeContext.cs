using Bridge.Domain.Places.Entities;
using Bridge.Domain.Products.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bridge.Infrastructure.Data
{
    public class BridgeContext : DbContext
    {
        public BridgeContext(DbContextOptions<BridgeContext> options) : base(options)
        {
        }

        public DbSet<Place> Places { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BridgeContext).Assembly);
        }

    }
}
