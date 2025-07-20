using TransferService.Domain.ValueObjects;
using TransferService.Domain.Common;
using TransferService.Domain.Events;

namespace TransferService.Domain.Entities
{
    public class Transfer : BaseEntity
    {
        public Guid Id { get; private set; }
        public Guid SenderUserId { get; private set; }
        public Guid ReceiverUserId { get; private set; }
        public Money Amount { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Transfer(Guid senderUserId, Guid receiverUserId, Money amount)
        {
            Id = Guid.NewGuid();
            SenderUserId = senderUserId;
            ReceiverUserId = receiverUserId;
            Amount = amount;
            CreatedAt = DateTime.UtcNow;

            AddDomainEvent(new TransferCreatedDomainEvent(Id, SenderUserId, ReceiverUserId, Amount.Amount, Amount.Currency));
        }

        private Transfer()
        {
            // Parameterless constructor for EF
        }
    }
}
