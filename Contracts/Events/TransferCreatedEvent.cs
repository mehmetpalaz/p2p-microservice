namespace Contracts.Events
{
    public class TransferCreatedEvent
    {
        public Guid TransferId { get; set; }
        public Guid SenderUserId { get; set; }
        public Guid ReceiverUserId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
