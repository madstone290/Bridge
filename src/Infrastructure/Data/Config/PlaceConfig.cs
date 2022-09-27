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
            builder.OwnsOne(x => x.Location);

            // PlaceType
            builder.Property(x => x.Type)
                .HasConversion<string>();

            // OpeningTimes
            var timeBuilder = builder.OwnsMany(x => x.OpeningTimes);
            timeBuilder.HasKey(x => x.Id);
            timeBuilder.Property(x => x.Day)
                .HasConversion<string>();

            // Categories
            var categoryBuilder = builder.OwnsMany(x => x.CategoryItems);
            categoryBuilder.HasKey(x => x.Id);
            categoryBuilder.Property(x => x.Category)
                .HasConversion<string>();
        }
    }
}
