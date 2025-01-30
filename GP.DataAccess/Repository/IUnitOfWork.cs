namespace GP.DataAccess.Repository
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        Task CompleteAsync(CancellationToken cancellationToken);
        Task CompleteAsync();
        Task BeginTransactionAsync(CancellationToken cancellationToken);
        Task BeginTransactionAsync();
        Task RollbackTransactionAsync(CancellationToken cancellationToken);
        Task RollbackTransactionAsync();
        void Complete();

    }
}
