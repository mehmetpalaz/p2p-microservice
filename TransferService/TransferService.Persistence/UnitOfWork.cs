using MediatR;
using TransferService.Application.Interfaces;
using TransferService.Domain.Common;
using TransferService.Persistence.Contexts;

namespace TransferService.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TransferDbContext _dbContext;
        private readonly IMediator _mediator;

        public UnitOfWork(TransferDbContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            // Dispatch domain events BEFORE SaveChanges
            var domainEvents = _dbContext.ChangeTracker
                .Entries<BaseEntity>()
                .Where(x => x.Entity.DomainEvents.Any())
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            // First publish domain events (this adds messages to Outbox)
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent, cancellationToken);
            }

            // Clear domain events
            foreach (var entity in _dbContext.ChangeTracker.Entries<BaseEntity>())
            {
                entity.Entity.ClearDomainEvents();
            }

            // Then save to database (this saves both entities and outbox messages)
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
