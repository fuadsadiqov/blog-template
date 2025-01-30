using GP.Data;

namespace GP.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        private bool _disposed;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CompleteAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken)
        {
            await _context.Database.BeginTransactionAsync(cancellationToken);
        }
        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public Task RollbackTransactionAsync(CancellationToken cancellationToken)
        {
            return _context.Database.RollbackTransactionAsync(cancellationToken);
        }
        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public void Complete()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();
            _disposed = true;
        }

        public virtual ValueTask DisposeAsync()
        {
            return _context.DisposeAsync();

        }
    }
}
