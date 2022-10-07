using Bridge.Application.Common;
using Bridge.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bridge.Infrastructure.Data
{
    public abstract class ReadRepository<TEntity> : IReadRepository<TEntity> where TEntity : Entity
    {
        protected readonly DbSet<TEntity> Set;
        public ReadRepository(BridgeContext context)
        {
            Set = context.Set<TEntity>();
        }
    }

    public abstract class ReadRepository<TEntity, TReadModel> : ReadRepository<TEntity>, IReadRepository<TEntity, TReadModel> where TEntity : Entity
    {
        protected ReadRepository(BridgeContext context) : base(context)
        {
        }

        public abstract Expression<Func<TEntity, TReadModel>> SelectExpression { get; }

        public async Task<TReadModel?> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Set
                .Where(predicate)
                .Select(SelectExpression)
                .FirstOrDefaultAsync();
        }

        public async Task<List<TReadModel>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Set
               .Where(predicate)
               .Select(SelectExpression)
               .ToListAsync();
        }
    }
}
