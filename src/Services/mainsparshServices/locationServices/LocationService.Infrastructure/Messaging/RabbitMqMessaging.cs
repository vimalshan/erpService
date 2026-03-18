using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace LocationService.Infrastructure.Messaging
{
    /// <summary>
    /// RabbitMQ message publisher interface
    /// </summary>
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(string queueName, T message, CancellationToken cancellationToken = default) where T : class;
    }

    /// <summary>
    /// RabbitMQ message publisher implementation
    /// </summary>
    public class RabbitMqMessagePublisher : IMessagePublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<RabbitMqMessagePublisher> _logger;

        public RabbitMqMessagePublisher(IConnection connection, ILogger<RabbitMqMessagePublisher> logger)
        {
            _connection = connection;
            _channel = connection.CreateModel();
            _logger = logger;
        }

        public async Task PublishAsync<T>(string queueName, T message, CancellationToken cancellationToken = default) where T : class
        {
            try
            {
                _channel.QueueDeclare(
                    queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);

                var properties = _channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.ContentType = "application/json";

                _channel.BasicPublish(
                    exchange: "",
                    routingKey: queueName,
                    basicProperties: properties,
                    body: body);

                _logger.LogInformation("Message published to queue {QueueName}", queueName);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing message to queue {QueueName}", queueName);
                throw;
            }
        }
    }

    /// <summary>
    /// RabbitMQ message consumer base class
    /// </summary>
    public abstract class RabbitMqConsumerBase
    {
        protected readonly IModel Channel;
        protected readonly ILogger Logger;

        protected RabbitMqConsumerBase(IConnection connection, ILogger logger)
        {
            Channel = connection.CreateModel();
            Logger = logger;
        }

        public virtual async Task StartListeningAsync(string queueName, CancellationToken cancellationToken = default)
        {
            Channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            Channel.BasicQos(0, 1, false);

            var consumer = new AsyncEventingBasicConsumer(Channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);
                    await HandleMessageAsync(json, cancellationToken);
                    Channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Error processing message");
                    Channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            Channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            Logger.LogInformation("Listening on queue {QueueName}", queueName);

            await Task.CompletedTask;
        }

        protected abstract Task HandleMessageAsync(string message, CancellationToken cancellationToken);
    }
}
