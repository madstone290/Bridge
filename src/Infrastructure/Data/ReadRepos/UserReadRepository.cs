using Bridge.Application.Users.ReadModels;
using Bridge.Application.Users.ReadRepos;
using Bridge.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bridge.Infrastructure.Data.ReadRepos
{
    public class UserReadRepository : Repository<User>, IUserReadRepository
    {
        public UserReadRepository(BridgeContext context) : base(context)
        {
        }

        public async Task<UserReadModel?> GetAdminUserAsync(long id)
        {
            return await Set
                .Where(x => x.Id == id && x.IsAdmin)
                .SelectUserReadModel()
                .FirstOrDefaultAsync();
        }

        public async Task<UserReadModel?> GetUserAsync(long id)
        {
            return await Set
                .Where(x => x.Id == id && x.IsAdmin)
                .SelectUserReadModel()
                .FirstOrDefaultAsync();
        }
    }

    public static class UserReadRepositoryExtensions
    {
        public static IQueryable<UserReadModel> SelectUserReadModel(this IQueryable<User> query)
        {
            return query
                .Select(x => new UserReadModel()
                {
                    Id = x.Id,
                    IsAdmin = x.IsAdmin,
                    Name = x.Name,
                    IdentityUserId = x.IdentityUserId
                });
        }
    }
}

