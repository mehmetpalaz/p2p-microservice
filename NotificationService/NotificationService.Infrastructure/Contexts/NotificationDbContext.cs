using Microsoft.EntityFrameworkCore;
using NotificationService.Infrastructure.Inbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Contexts
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
        {
        }

        public DbSet<InboxMessage> InboxMessages => Set<InboxMessage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InboxMessage>(cfg =>
            {
                cfg.HasKey(x => x.Id);
                cfg.Property(x => x.Name).IsRequired();
                cfg.Property(x => x.ReceivedAt).IsRequired();
                cfg.Property(x => x.Consumer).IsRequired();
            });
        }

    }
}
