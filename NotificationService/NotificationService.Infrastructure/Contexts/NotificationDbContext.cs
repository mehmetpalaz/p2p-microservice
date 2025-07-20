using MassTransit;
using Microsoft.EntityFrameworkCore;


namespace NotificationService.Infrastructure.Contexts
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxStateEntity(); 
        }

    }
}
