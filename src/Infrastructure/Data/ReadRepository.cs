using Bridge.Application.Common;
using Bridge.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bridge.Infrastructure.Data
{
    public abstract class ReadRepository<TEntity, TReadModel> : IReadRepository<TEntity, TReadModel> where TEntity : Entity, IAggregateRoot
    {
        protected readonly DbSet<TEntity> Set;
        public ReadRepository(BridgeContext context)
        {
            Set = context.Set<TEntity>();
        }

        public async Task<List<TReadModel>> FilterAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Set
               .Where(predicate)
               .Select(SelectExpression)
               .ToListAsync();
        }

        protected abstract Expression<Func<TEntity, TReadModel>> SelectExpression { get; }
    }
}
