using Bridge.Application.Users.ReadModels;
using Bridge.Application.Users.ReadRepos;
using Bridge.Domain.Users.Entities;
using System.Linq.Expressions;

namespace Bridge.Infrastructure.Data.ReadRepos
{
    public class UserReadRepository : ReadRepository<User, UserReadModel>, IUserReadRepository
    {
        public UserReadRepository(BridgeContext context) : base(context)
        {
        }

        protected override Expression<Func<User, UserReadModel>> SelectExpression { get; } = x => new UserReadModel()
        {
            Id = x.Id,
            IsAdmin = x.IsAdmin,
            Name = x.Name,
            IdentityUserId = x.IdentityUserId
        };

        public async Task<UserReadModel?> GetAdminUserAsync(long id)
        {
            return (await FilterAsync(x => x.Id == id && x.IsAdmin))
                .FirstOrDefault();
        }

        public async Task<UserReadModel?> GetUserAsync(long id)
        {
            return (await FilterAsync(x => x.Id == id))
              .FirstOrDefault();
        }

    }
}

