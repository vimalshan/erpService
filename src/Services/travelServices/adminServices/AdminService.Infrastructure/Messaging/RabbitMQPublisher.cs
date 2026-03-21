using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace AdminService.Infrastructure.Messaging;

/// <summary>
/// Service for publishing messages to RabbitMQ
/// </summary>
public interface IMessagePublisher
{
    Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default);
}

/// <summary>
/// Implementation of RabbitMQ message publisher
/// </summary>
public class RabbitMQPublisher : IMessagePublisher
{
    private readonly Lazy<IConnection> _connection;
    private IModel? _channel;
    private readonly ILogger<RabbitMQPublisher> _logger;

    public RabbitMQPublisher(Lazy<IConnection> connection, ILogger<RabbitMQPublisher> logger)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default)
    {
        try
        {
            // Lazy initialization of channel on first use
            _channel ??= _connection.Value.CreateModel();
            
            _channel.ExchangeDeclare(exchange, ExchangeType.Topic, durable: true, autoDelete: false);

            var messageJson = System.Text.Json.JsonSerializer.Serialize(message);
            var body = System.Text.Encoding.UTF8.GetBytes(messageJson);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "application/json";

            _channel.BasicPublish(exchange, routingKey, properties, body);
            _logger.LogInformation("Message published to exchange: {Exchange}, routing key: {RoutingKey}", exchange, routingKey);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing message to RabbitMQ");
            throw;
        }
    }
}
