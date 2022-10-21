using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Bridge.Domain.Common
{
    public interface IRepository<T> where T : Entity, IAggregateRoot
    {
        Task<bool> ExistByIdAsync(long id);

        Task<T?> FindByIdAsync(long id);

        Task AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);

        void Remove(T entity);

    }
}
