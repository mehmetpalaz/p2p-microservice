using TransferService.Application.Repositories;
using TransferService.Domain.Entities;
using TransferService.Persistence.Contexts;

namespace TransferService.Persistence.Repositories
{
    public class TransferRepository : ITransferRepository
    {
        private readonly TransferDbContext _dbContext;

        public TransferRepository(TransferDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Transfer transfer, CancellationToken cancellationToken)
        {
            await _dbContext.Transfers.AddAsync(transfer, cancellationToken);
        }
    }
}
