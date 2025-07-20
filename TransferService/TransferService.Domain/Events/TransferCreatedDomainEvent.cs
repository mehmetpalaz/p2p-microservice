using TransferService.Domain.Common;

namespace TransferService.Domain.Events
{
    public class TransferCreatedDomainEvent : IDomainEvent
    {
        public Guid TransferId { get; }
        public Guid SenderUserId { get; }
        public Guid ReceiverUserId { get; }
        public decimal Amount { get; }
        public string Currency { get; }
        public DateTime OccurredAt { get; }

        public TransferCreatedDomainEvent(Guid transferId, Guid senderUserId, Guid receiverUserId, decimal amount, string currency)
        {
            TransferId = transferId;
            SenderUserId = senderUserId;
            ReceiverUserId = receiverUserId;
            Amount = amount;
            Currency = currency;
            OccurredAt = DateTime.UtcNow;
        }
    }
}