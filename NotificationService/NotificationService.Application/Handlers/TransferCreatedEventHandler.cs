using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Abstractions;
using NotificationService.Domain.Events;
using NotificationService.Infrastructure.Contexts;
using NotificationService.Infrastructure.Inbox;
using Npgsql;


namespace NotificationService.Application.Handlers
{
    public class TransferCreatedEventHandler : ITransferCreatedEventHandler
    {
        private readonly IServiceProvider _serviceProvider;

        public TransferCreatedEventHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task HandleAsync(TransferCreatedEvent @event)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();

            Console.WriteLine($"📨 [NotificationService] Transfer Event Received: {@event}");

            var alreadyProcessed = await dbContext.InboxMessages
                .AnyAsync(x => x.Id == @event.Id);

            if (alreadyProcessed)
            {
                Console.WriteLine($"⚠️ [NotificationService] Duplicate event detected. Skipping: {@event.Id}");
                return;
            }

            if (@event?.Currency != "TRY")
            {
                throw new Exception($"Unsupported currency: {@event?.Currency}");
            }

            Console.WriteLine($"✅ [NotificationService] Transfer completed. Notification sent to user {@event.ReceiverUserId} for {@event.Amount} {@event.Currency}");

            var inboxMessage = new InboxMessage
            {
                Id = @event.Id,
                Name = nameof(TransferCreatedEvent),
                Consumer = "message",
                ReceivedAt = @event.CreatedAt,
                ProcessedAt = DateTime.UtcNow
            };

            dbContext.InboxMessages.Add(inboxMessage);
            
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
            {
                Console.WriteLine("⚠️ [InboxService] Duplicate message detected, skipping insert.");
            }
        }
    }
}