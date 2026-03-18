using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace AccessService.Infrastructure.MessageBrokers.RabbitMQ
{
    /// <summary>
    /// RabbitMQ message publisher implementation
    /// Publishes messages to exchanges and queues
    /// </summary>
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly IRabbitMQConnection _connection;
        private readonly ILogger<RabbitMQPublisher> _logger;

        public RabbitMQPublisher(IRabbitMQConnection connection, ILogger<RabbitMQPublisher> logger)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PublishAsync(string exchangeName, string routingKey, string message, Dictionary<string, object> headers = null)
        {
            try
            {
                var channel = await _connection.GetChannelAsync();

                // Declare exchange
                channel.ExchangeDeclare(
                    exchange: exchangeName,
                    type: ExchangeType.Topic,
                    durable: true,
                    autoDelete: false,
                    arguments: null);

                // Prepare properties
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.ContentType = "application/json";
                properties.MessageId = Guid.NewGuid().ToString();
                properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

                if (headers != null)
                {
                    properties.Headers = headers;
                }

                // Publish message
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(
                    exchange: exchangeName,
                    routingKey: routingKey,
                    basicProperties: properties,
                    body: body);

                _logger.LogInformation($"Message published to exchange '{exchangeName}' with routing key '{routingKey}'. MessageId: {properties.MessageId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error publishing message to exchange '{exchangeName}'");
                throw;
            }
        }

        public async Task PublishAsync(string queueName, string message, Dictionary<string, object> headers = null)
        {
            try
            {
                var channel = await _connection.GetChannelAsync();

                // Declare queue
                channel.QueueDeclare(
                    queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                // Prepare properties
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.ContentType = "application/json";
                properties.MessageId = Guid.NewGuid().ToString();
                properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

                if (headers != null)
                {
                    properties.Headers = headers;
                }

                // Publish message
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(
                    exchange: "",
                    routingKey: queueName,
                    basicProperties: properties,
                    body: body);

                _logger.LogInformation($"Message published to queue '{queueName}'. MessageId: {properties.MessageId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error publishing message to queue '{queueName}'");
                throw;
            }
        }
    }
}
