using GP.Infrastructure.Configurations;
using GP.Data;
using MediatR;

namespace GP.Infrastructure.Pipelines
{
    public class TransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where
        TRequest
        : ITransactionalRequest
    {
        private readonly ApplicationDbContext _dbContext;

        public TransactionPipelineBehavior(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentException(nameof(dbContext));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                var currentTransaction = _dbContext.Database.CurrentTransaction;
                var isFirstTransaction = currentTransaction == null;
                if (isFirstTransaction)
                {
                    await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                }

                var response = await next();

                if (isFirstTransaction)
                {
                    await _dbContext.Database.CommitTransactionAsync(cancellationToken);

                }


                return response;
            }
            catch (Exception)
            {
                var isTransactionExist = _dbContext.Database.CurrentTransaction != null;

                if (isTransactionExist)
                {
                    await _dbContext.Database.RollbackTransactionAsync(cancellationToken);
                }

                throw;
            }
        }
    }
}