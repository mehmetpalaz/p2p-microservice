using Microsoft.AspNetCore.Connections;
using NotificationService.Application.Abstractions;
using NotificationService.Application.Handlers;
using NotificationService.Domain.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace NotificationService.API.Messaging
{
    public class TransferCreatedConsumer : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;

        private readonly IServiceProvider _serviceProvider;

        public TransferCreatedConsumer(IServiceProvider serviceProvider)
        {
                _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                UserName = "guest",
                Password = "guest"
            };

            CreateConnection(factory);

            DeclareAndBindQueue();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var transferEvent = JsonSerializer.Deserialize<TransferCreatedEvent>(message);

                    using var scope = _serviceProvider.CreateScope();
                    
                    var _handler = scope.ServiceProvider.GetRequiredService<ITransferCreatedEventHandler>();

                    await _handler.HandleAsync(transferEvent!);
                    
                    _channel.BasicAck(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"🔥 [NotificationService] Error: {ex.Message}");
                   
                    _channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
                }
            };

            _channel.BasicConsume(queue: "notification_queue", autoAck: false, consumer: consumer);
        }

        private void DeclareAndBindQueue()
        {
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("transfer_exchange", ExchangeType.Fanout, durable: true);

            _channel.QueueDeclare("notification_queue", durable: true, exclusive: false, autoDelete: false,
                arguments: new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "dlx.exchange" },
                { "x-dead-letter-routing-key", "transfer-created-dlq" }
            });

            _channel.QueueBind("notification_queue", "transfer_exchange", "");

            _channel.ExchangeDeclare("dlx.exchange", ExchangeType.Direct, durable: true);

            _channel.QueueDeclare(queue: "transfer-created-dlq",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.QueueBind("transfer-created-dlq", "dlx.exchange", "transfer-created-dlq");
        }

        private void CreateConnection(ConnectionFactory factory)
        {
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
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }
    }
}
