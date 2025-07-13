using RabbitMQ.Client;
using System.Text;

namespace TransferService.Infrastructure.Messaging
{
    public interface IRabbitMqEventPublisher
    {
        void Publish(string message, string messageType);
    }

    public class RabbitMqEventPublisher : IRabbitMqEventPublisher
    {
        private readonly ConnectionFactory _factory;

        public RabbitMqEventPublisher()
        {
            _factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                UserName = "guest",
                Password = "guest"
            };
        }

        public void Publish(string message, string messageType)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare("transfer_exchange", ExchangeType.Fanout, durable: true);

            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Type = messageType;

            channel.BasicPublish(
                exchange: "transfer_exchange",
                routingKey: "",
                basicProperties: properties,
                body: body
            );
        }
    }

}
