using Bridge.Domain.Users.Entities;
using Bridge.Domain.Users.Repos;
using Microsoft.EntityFrameworkCore;

namespace Bridge.Infrastructure.Data.Repos
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(BridgeContext context) : base(context)
        {
        }

        public async Task<User?> FindByIdentityUserId(string identityUserId)
        {
            return await Set.FirstOrDefaultAsync(x => x.IdentityUserId == identityUserId);
        }
    }
}
