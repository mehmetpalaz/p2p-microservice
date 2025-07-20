namespace TransferService.Infrastructure.Configurations
{
    public class RabbitMqOptions
    {
        public string Host { get; set; } = default!;
        public string Username { get; set; } = "guest";
        public string Password { get; set; } = "guest";
    }
}
