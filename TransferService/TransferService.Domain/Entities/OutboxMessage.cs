namespace TransferService.Domain.Entities
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = default!;
        public string Content { get; set; } = default!;
        public DateTime OccurredOn { get; set; } = DateTime.UtcNow;
        public bool Processed { get; set; } = false;
    }
}
