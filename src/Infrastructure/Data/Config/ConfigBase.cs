using Bridge.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bridge.Infrastructure.Data.Config
{
    public abstract class ConfigBase<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : Entity
    {
        public abstract void Configure(EntityTypeBuilder<TEntity> builder);

        void IEntityTypeConfiguration<TEntity>.Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Pk);

            builder.Property(x => x.Id).IsRequired();

            Configure(builder);
        }

    }
}
