using Contracts.Events;
using MediatR;
using TransferService.Domain.Events;
using TransferService.Infrastructure.Messaging;

namespace TransferService.Application.DomainEventHandlers
{
    public class TransferCreatedDomainEventHandler : INotificationHandler<TransferCreatedDomainEvent>
    {
        private readonly IEventPublisher _eventPublisher;

        public TransferCreatedDomainEventHandler(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public async Task Handle(TransferCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var integrationEvent = new TransferCreatedEvent
            {
                TransferId = notification.TransferId,
                SenderUserId = notification.SenderUserId,
                ReceiverUserId = notification.ReceiverUserId,
                Amount = notification.Amount,
                Currency = notification.Currency,
                CreatedAt = notification.OccurredAt
            };

            await _eventPublisher.PublishAsync(integrationEvent);
        }
    }
}