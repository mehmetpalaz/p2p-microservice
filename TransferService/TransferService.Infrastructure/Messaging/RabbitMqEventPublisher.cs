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
        private readonly IConnection _connection;

        public RabbitMqEventPublisher()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
        }

        public void Publish(string message, string messageType)
        {
            using var channel = _connection.CreateModel();
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
