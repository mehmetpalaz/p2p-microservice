using MassTransit;
using Microsoft.Extensions.Logging;

namespace TransferService.Infrastructure.Messaging
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T @event) where T : class;
    }

    public class EventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<EventPublisher> _logger;

        public EventPublisher(IPublishEndpoint publishEndpoint, ILogger<EventPublisher> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task PublishAsync<T>(T @event) where T : class
        {
            try
            {                
                await _publishEndpoint.Publish(@event);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ [EventPublisher] Failed to publish event of type {EventType}: {@Event}", 
                    typeof(T).Name, @event);

                throw;
            }
        }
    }
}
