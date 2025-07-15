using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Inbox
{
    public class InboxMessage
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime ReceivedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string Consumer { get; set; } = null!;
    }

}
