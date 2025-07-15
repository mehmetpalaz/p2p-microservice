using NotificationService.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Handlers
{
    public class TransferCreatedEventHandler
    {
        public Task HandleAsync(TransferCreatedEvent @event)
        {

            Console.WriteLine($"📨 [NotificationService] Transfer Event Received: {@event}");

            // for testing error queue
            if (@event?.Currency != "TRY")
            {
                throw new Exception($"Unsupported currency: {@event?.Currency}");
            }

            Console.WriteLine($"✅ [NotificationService] Transfer completed. Notification sent to user {@event.ReceiverUserId} for {@event.Amount} {@event.Currency}");


            return Task.CompletedTask;
        }

    }
}