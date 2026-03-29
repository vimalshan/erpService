using InsuranceManagement.Infrastructure.MessageConsumers;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace InsuranceManagement.Infrastructure.Messaging;

/// <summary>
/// Interface for publishing insurance domain events to RabbitMQ.
/// </summary>
public interface IInsuranceMessagePublisher : IDisposable
{
    void Publish<T>(string routingKey, T message) where T : class;
}

/// <summary>
/// RabbitMQ publisher for insurance domain events using v6 sync API.
/// Publishes to the "insurance.events" topic exchange.
/// </summary>
public class InsuranceRabbitMqPublisher : IInsuranceMessagePublisher
{
    private const string Exchange = "insurance.events";
    private readonly IRabbitMqConnectionFactory _connectionFactory;
    private readonly ILogger<InsuranceRabbitMqPublisher> _logger;
    private IConnection? _connection;
    private IModel? _channel;
    private readonly object _lock = new();

    public InsuranceRabbitMqPublisher(
        IRabbitMqConnectionFactory connectionFactory,
        ILogger<InsuranceRabbitMqPublisher> logger)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Publish<T>(string routingKey, T message) where T : class
    {
        try
        {
            EnsureConnection();

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = _channel!.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "application/json";
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            _channel.BasicPublish(
                exchange: Exchange,
                routingKey: routingKey,
                basicProperties: properties,
                body: body);

            _logger.LogInformation(
                "Published message to exchange {Exchange} with routing key {RoutingKey}",
                Exchange, routingKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish message to RabbitMQ with routing key {RoutingKey}", routingKey);
        }
    }

    private void EnsureConnection()
    {
        lock (_lock)
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = _connectionFactory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(
                    exchange: Exchange,
                    type: ExchangeType.Topic,
                    durable: true,
                    autoDelete: false);
            }
        }
    }

    public void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        _connection?.Close();
        _connection?.Dispose();
    }
}
