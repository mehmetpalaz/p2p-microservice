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

            var @event = new
            {
                transfer.Id,
                transfer.SenderUserId,
                transfer.ReceiverUserId,
                transfer.Amount.Amount,
                transfer.Amount.Currency,
                transfer.CreatedAt
            };

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = "TransferCreated",
                Content = System.Text.Json.JsonSerializer.Serialize(@event),
                OccurredOn = DateTime.UtcNow
            };

            await _dbContext.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
        }
    }
}
