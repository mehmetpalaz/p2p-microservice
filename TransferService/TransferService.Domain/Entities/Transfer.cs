using TransferService.Domain.ValueObjects;

namespace TransferService.Domain.Entities
{
    public class Transfer
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
        }
    }

}
