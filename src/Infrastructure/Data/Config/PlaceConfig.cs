using Bridge.Domain.Places.Entities;
using Bridge.Shared.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace Bridge.Infrastructure.Data.Config
{
    public class PlaceConfig : IEntityTypeConfiguration<Place>
    {
        public void Configure(EntityTypeBuilder<Place> builder)
        {
            // Location
            var locationBuilder = builder.OwnsOne(x => x.Location);
            locationBuilder.Property(x => x.Latitude)
                .HasConversion(new ValueConverter<decimal, string>(
                    value => value.ToString(),
                    providerValue => decimal.Parse(providerValue))
                );
            locationBuilder.Property(x => x.Longitude)
               .HasConversion(new ValueConverter<decimal, string>(
                   value => value.ToString(),
                   providerValue => decimal.Parse(providerValue))
               );

            // OpeningTimes
            var timeBuilder = builder.OwnsMany(x => x.OpeningTimes);
            timeBuilder.HasKey(x => x.Id);
            timeBuilder.Property(x => x.Day)
                .HasConversion<string>();

            // Categories
            builder.Property(x => x.Categories)
                .HasConversion(new ValueConverter<IEnumerable<PlaceCategory>, string>(
                    value => JsonSerializer.Serialize(value, JsonOptions.Default),
                    providerValue => JsonSerializer.Deserialize<HashSet<PlaceCategory>>(providerValue, JsonOptions.Default) ?? new HashSet<PlaceCategory>())
                );
        }
    }
}
