using Bridge.Domain.Products.Entities;
using Bridge.Shared.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Bridge.Infrastructure.Data.Config
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Id
            builder.HasKey(x => x.Id);

            // Place
            builder.HasOne(x => x.Place)
                .WithMany()
                .HasForeignKey(x => x.PlaceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Price
            builder.Property(x => x.Price)
                .HasPrecision(18, 4);

            // Categories
            var categoryBuilder = builder.OwnsMany(x => x.CategoryItems);
            categoryBuilder.HasKey(x => x.Id);
            categoryBuilder.Property(x => x.Category)
                .HasConversion<string>();
        }
    }
}
