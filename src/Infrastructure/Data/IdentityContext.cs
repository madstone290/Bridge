using Bridge.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bridge.Infrastructure.Data
{
    public class IdentityContext : IdentityDbContext<BridgeUser, BridgeRole, long>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BridgeUser>().OwnsOne(x => x.UserDetails)
                .Property(x=> x.UserType).HasConversion<string>();

            base.OnModelCreating(builder);
        }
    }
}
