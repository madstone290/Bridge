using Bridge.Application.Common;

namespace Bridge.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly BridgeContext _context;

        public UnitOfWork(BridgeContext context)
        {
            _context = context;
        }

        public Task CommitAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
