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
            // PlaceType
            builder.Property(x => x.Type)
                .HasConversion<string>();

            builder.Property(x => x.Status)
                .HasConversion<string>();

            // Location
            builder.OwnsOne(x => x.Location);

            // Address
            builder.OwnsOne(x => x.Address);

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
