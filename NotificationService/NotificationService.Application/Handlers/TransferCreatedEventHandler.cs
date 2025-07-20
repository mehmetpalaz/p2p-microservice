using Contracts.Events;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Abstractions;

namespace NotificationService.Application.Handlers
{
    public class TransferCreatedEventHandler : ITransferCreatedEventHandler
    {
        private readonly ILogger<TransferCreatedEventHandler> _logger;

        public TransferCreatedEventHandler(ILogger<TransferCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(TransferCreatedEvent @event)
        {
            _logger.LogInformation("📨 [NotificationService] Transfer Event Received: TransferId={TransferId}, Amount={Amount} {Currency}",
                @event.TransferId, @event.Amount, @event.Currency);

            try
            {
                // Business validation
                if (@event?.Currency != "TRY")
                {
                    _logger.LogWarning("Unsupported currency: {Currency} for TransferId={TransferId}",
                        @event?.Currency, @event?.TransferId);
                    throw new NotSupportedException($"Unsupported currency: {@event?.Currency}");
                }

                // Simulate notification sending
                await SimulateNotificationSending(@event);

                _logger.LogInformation("✅ [NotificationService] Transfer completed. Notification sent to user {ReceiverUserId} for {Amount} {Currency}",
                    @event.ReceiverUserId, @event.Amount, @event.Currency);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ [NotificationService] Error processing transfer event for TransferId={TransferId}",
                    @event?.TransferId);
                throw;
            }
        }

        private static async Task SimulateNotificationSending(TransferCreatedEvent @event)
        {
            // Simulate some async work (email, SMS, push notification etc.)
            await Task.Delay(100);

            // Here you would integrate with real notification services
            // - Email service
            // - SMS service  
            // - Push notification service
        }
    }
}