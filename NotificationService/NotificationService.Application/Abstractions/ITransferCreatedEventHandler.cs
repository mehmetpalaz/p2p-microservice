using NotificationService.Domain.Events;

namespace NotificationService.Application.Abstractions
{
    public interface ITransferCreatedEventHandler : IEventHandler<TransferCreatedEvent>
    {
    }
}
