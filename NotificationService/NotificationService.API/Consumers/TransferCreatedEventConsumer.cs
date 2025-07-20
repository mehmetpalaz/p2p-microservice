using Contracts.Events;
using MassTransit;
using NotificationService.Application.Abstractions;

namespace NotificationService.API.Consumers
{
    public class TransferCreatedEventConsumer : IConsumer<TransferCreatedEvent>
    {
        private readonly ITransferCreatedEventHandler _handler;

        public TransferCreatedEventConsumer(ITransferCreatedEventHandler handler)
        {
            _handler = handler;
        }

        public async Task Consume(ConsumeContext<TransferCreatedEvent> context)
        {
            await _handler.HandleAsync(context.Message);
        }
    }

}
