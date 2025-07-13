using TransferService.Application.Interfaces;
using TransferService.Persistence.Contexts;

namespace TransferService.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TransferDbContext _dbContext;

        public UnitOfWork(TransferDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

}
