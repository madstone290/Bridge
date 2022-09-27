using Bridge.Domain.Places.Entities;
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

            // ProductType
            builder.Property(x => x.Type)
                .HasConversion<string>();

            // Price
            builder.Property(x => x.Price)
                .HasPrecision(18, 4);

            // Categories
            builder.Property(x => x.Categories)
                .HasConversion(new ValueConverter<IEnumerable<ProductCategory>, string>(
                    value => JsonSerializer.Serialize(value, JsonOptions.Default),
                    providerValue => JsonSerializer.Deserialize<HashSet<ProductCategory>>(providerValue, JsonOptions.Default) ?? new HashSet<ProductCategory>())
                );

        }
    }
}
