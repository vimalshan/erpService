using AuthProvider.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace AuthProvider.Infrastructure.Services;

/// <summary>
/// RabbitMQ message publisher implementation.
/// Uses the direct-exchange pattern to publish domain events as JSON messages.
/// Implements Circuit Breaker + Retry via Polly policies (see Infrastructure DI registration).
/// </summary>
public sealed class RabbitMQPublisher : IMessagePublisher, IDisposable
{
    private readonly IConfiguration _config;
    private readonly ILogger<RabbitMQPublisher> _logger;
    private IConnection? _connection;
    private IModel? _channel;
    private bool _disposed;

    private const string ExchangeName = "auth.events";

    public RabbitMQPublisher(IConfiguration config, ILogger<RabbitMQPublisher> logger)
    {
        _config = config;
        _logger = logger;
        InitialiseConnection();
    }

    private void InitialiseConnection()
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _config["RabbitMQ:Host"] ?? "localhost",
                Port = int.TryParse(_config["RabbitMQ:Port"], out var port) ? port : 5672,
                UserName = _config["RabbitMQ:Username"] ?? "guest",
                Password = _config["RabbitMQ:Password"] ?? "guest",
                VirtualHost = _config["RabbitMQ:VirtualHost"] ?? "/",
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection("AuthProvider.API");
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(ExchangeName, ExchangeType.Topic, durable: true, autoDelete: false);
            _logger.LogInformation("RabbitMQ connection established to {Host}", factory.HostName);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "RabbitMQ unavailable – messages will be silently dropped in development.");
        }
    }

    public async Task PublishAsync<T>(T message, CancellationToken ct = default) where T : class
    {
        if (_channel is null || !_channel.IsOpen)
        {
            _logger.LogWarning("RabbitMQ channel not available. Skipping publish of {MessageType}", typeof(T).Name);
            return;
        }

        var routingKey = $"auth.{typeof(T).Name.ToLowerInvariant()}";
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));

        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.ContentType = "application/json";
        properties.MessageId = Guid.NewGuid().ToString();
        properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        _channel.BasicPublish(ExchangeName, routingKey, properties, body);
        _logger.LogDebug("Published {MessageType} to exchange {Exchange} with routing key {RoutingKey}",
            typeof(T).Name, ExchangeName, routingKey);

        await Task.CompletedTask;
    }

    public void Dispose()
    {
        if (_disposed) return;
        _channel?.Close();
        _channel?.Dispose();
        _connection?.Close();
        _connection?.Dispose();
        _disposed = true;
    }
}
