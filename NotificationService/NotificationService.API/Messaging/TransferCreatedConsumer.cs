using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Text;

namespace NotificationService.API.Messaging
{
    public class TransferCreatedConsumer : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                UserName = "guest",
                Password = "guest"
            };

            int retryCount = 5;

            while (retryCount > 0)
            {
                try
                {
                    _connection = factory.CreateConnection();

                    break;
                }
                catch (BrokerUnreachableException)
                {
                    retryCount--;
                    Thread.Sleep(2000);
                    if (retryCount == 0)
                        throw;
                }
            }

            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("transfer_exchange", ExchangeType.Fanout, durable: true);
            _channel.QueueDeclare("notification_queue", durable: true, exclusive: false, autoDelete: false);
            _channel.QueueBind("notification_queue", "transfer_exchange", "");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"📨 [NotificationService] Transfer Event Received: {message}");
            };

            _channel.BasicConsume(queue: "notification_queue", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }
    }

}
