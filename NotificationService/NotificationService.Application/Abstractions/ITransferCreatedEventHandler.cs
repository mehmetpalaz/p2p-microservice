using Contracts.Events;

namespace NotificationService.Application.Abstractions
{
    public interface ITransferCreatedEventHandler : IEventHandler<TransferCreatedEvent>
    {
    }
}
