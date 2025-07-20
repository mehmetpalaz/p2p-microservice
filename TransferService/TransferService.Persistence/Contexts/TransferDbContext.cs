using Microsoft.EntityFrameworkCore;
using TransferService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MassTransit;

namespace TransferService.Persistence.Contexts
{
    public class TransferDbContext : DbContext
    {
        public TransferDbContext(DbContextOptions<TransferDbContext> options) : base(options) { }
        public DbSet<Transfer> Transfers => Set<Transfer>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transfer>(builder =>
            {
                builder.HasKey(t => t.Id);
                builder.OwnsOne(t => t.Amount, nav =>
                 {
                     nav.Property(a => a.Amount)
                         .HasColumnName("Amount")
                         .IsRequired();

                     nav.Property(a => a.Currency)
                         .HasColumnName("Currency")
                         .IsRequired()
                         .HasMaxLength(3);
                 });
            });

            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();

            base.OnModelCreating(modelBuilder);
        }
    }
}
