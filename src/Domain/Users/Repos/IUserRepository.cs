using Bridge.Domain.Common;
using Bridge.Domain.Users.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Domain.Users.Repos
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> FindByIdentityUserId(string identityUserId);
    }
}
