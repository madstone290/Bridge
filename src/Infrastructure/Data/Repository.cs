using Bridge.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Bridge.Infrastructure.Data
{
    public class Repository<T> : IRepository<T> where T : Entity, IAggregateRoot
    {
        protected readonly DbSet<T> Set;
        public Repository(BridgeContext context)
        {
            Set = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await Set.AddAsync(entity);  
        }

        public void Remove(T entity)
        {
            Set.Remove(entity);
        }

        public async Task<bool> ExistByIdAsync(long id)
        {
            return await Set.AnyAsync(x => x.Id == id);
        }

        public virtual async Task<T?> FindByIdAsync(long id)
        {
            return await Set.FirstOrDefaultAsync(x => x.Id == id);
        }

    }
}
