namespace AccessService.Infrastructure.Messaging;

using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using AccessService.Domain;
using Microsoft.Extensions.Logging;

/// <summary>
/// RabbitMQ domain event publisher for publishing domain events to message broker
/// </summary>
public interface IRabbitMQPublisher
{
    Task PublishEventAsync<TEvent>(TEvent @event) where TEvent : IDomainEvent;
}

/// <summary>
/// Implementation of RabbitMQ domain event publisher
/// </summary>
public class RabbitMQPublisher : IRabbitMQPublisher, IDisposable
{
    private readonly IRabbitMQConnectionFactory _connectionFactory;
    private readonly ILogger<RabbitMQPublisher> _logger;
    private IConnection? _connection;
    private IModel? _channel;

    private const string EXCHANGE_NAME = "access_service_exchange";
    private const string EXCHANGE_TYPE = "topic";

    public RabbitMQPublisher(IRabbitMQConnectionFactory connectionFactory, ILogger<RabbitMQPublisher> logger)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Publishes a domain event to RabbitMQ
    /// </summary>
    public async Task PublishEventAsync<TEvent>(TEvent @event) where TEvent : IDomainEvent
    {
        try
        {
            EnsureConnection();
            
            var eventType = typeof(TEvent).Name;
            var routingKey = $"access.{eventType.ToLower()}";
            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = _channel!.CreateBasicProperties();
            properties.ContentType = "application/json";
            properties.DeliveryMode = 2; // Persistent message
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            _channel!.BasicPublish(EXCHANGE_NAME, routingKey, properties, body);
            _logger.LogInformation($"Event published: {eventType} with routing key: {routingKey}");
            
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error publishing event: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Ensures connection and channel are established
    /// </summary>
    private void EnsureConnection()
    {
        if (_connection == null || !_connection.IsOpen)
        {
            _connection = _connectionFactory.CreateConnection();
        }

        if (_channel == null || _channel.IsClosed)
        {
            _channel = _connectionFactory.CreateChannel(_connection);
            
            // Declare exchange
            _channel.ExchangeDeclare(
                exchange: EXCHANGE_NAME,
                type: EXCHANGE_TYPE,
                durable: true,
                autoDelete: false,
                arguments: null
            );
            
            _logger.LogInformation($"RabbitMQ exchange '{EXCHANGE_NAME}' declared");
        }
    }

    /// <summary>
    /// Closes the connection and channel
    /// </summary>
    public void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        _connection?.Close();
        _connection?.Dispose();
        _logger.LogInformation("RabbitMQ connection and channel closed");
    }
}
