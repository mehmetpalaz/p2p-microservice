using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransferService.Infrastructure.Messaging;
using TransferService.Persistence.Contexts;

namespace TransferService.Infrastructure.Background
{
    public class OutboxPublisherService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IRabbitMqEventPublisher _publisher;

        public OutboxPublisherService(IServiceProvider serviceProvider, IRabbitMqEventPublisher publisher)
        {
            _serviceProvider = serviceProvider;
            _publisher = publisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<TransferDbContext>();

                var pendingMessages = await db.OutboxMessages
                    .Where(x => !x.Processed)
                    .OrderBy(x => x.OccurredOn)
                    .Take(10)
                    .ToListAsync(stoppingToken);

                foreach (var msg in pendingMessages)
                {
                    try
                    {
                        _publisher.Publish(msg.Content, msg.Type);

                        msg.Processed = true;
                        db.OutboxMessages.Update(msg);
                    }
                    catch
                    {
                        // log + continue
                    }
                }

                await db.SaveChangesAsync(stoppingToken);
                await Task.Delay(2000, stoppingToken);
            }
        }
    }

}
