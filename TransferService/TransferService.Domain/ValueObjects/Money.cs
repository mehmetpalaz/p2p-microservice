namespace TransferService.Domain.ValueObjects
{
    public record Money
    {
        public decimal Amount { get; init; }
        public string Currency { get; init; }

        public Money(decimal amount, string currency)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");
            Amount = amount;
            Currency = currency;
        }
    }

}
