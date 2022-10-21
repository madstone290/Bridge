using Bridge.Domain.Places.Entities.Places;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bridge.Infrastructure.Data.Config
{
    public class RestroomConfig : IEntityTypeConfiguration<Restroom>
    {
        public void Configure(EntityTypeBuilder<Restroom> builder)
        {
            builder.ToTable("Restrooms");

            builder.Property(x => x.DiaperTableLocation)
                .HasConversion<string>();
        }
    }
}
