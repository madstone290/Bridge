using Bridge.Domain.Products.Entities;
using Bridge.Domain.Products.Enums;
using Bridge.Shared.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Bridge.Infrastructure.Data.Config
{
    public class ProductConfig : ConfigBase<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            // ProductType
            builder.Property(x => x.Type)
                .HasConversion<string>();

            builder.Property(x => x.Status)
                .HasConversion<string>();

            // Place
            builder.HasOne(x => x.Place)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

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
