using ApiGateway.API.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace ApiGateway.API.Messaging;

public interface IGatewayEventPublisher
{
    Task PublishRouteEventAsync(string eventType, string serviceName, string detail, CancellationToken ct = default);
}

public sealed class RabbitMqGatewayEventPublisher : IGatewayEventPublisher, IAsyncDisposable
{
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<RabbitMqGatewayEventPublisher> _logger;
    private IConnection? _connection;
    private IChannel? _channel;
    private bool _initialized;

    public RabbitMqGatewayEventPublisher(
        IOptions<RabbitMqSettings> settings,
        ILogger<RabbitMqGatewayEventPublisher> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    private async Task EnsureInitializedAsync(CancellationToken ct)
    {
        if (_initialized) return;

        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost
            };

            _connection = await factory.CreateConnectionAsync(ct);
            _channel = await _connection.CreateChannelAsync(cancellationToken: ct);

            await _channel.ExchangeDeclareAsync(
                exchange: _settings.ExchangeName,
                type: ExchangeType.Topic,
                durable: true,
                cancellationToken: ct);

            _initialized = true;
            _logger.LogInformation("RabbitMQ connection established for gateway events");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to initialize RabbitMQ connection — gateway events will be skipped");
        }
    }

    public async Task PublishRouteEventAsync(string eventType, string serviceName, string detail, CancellationToken ct = default)
    {
        await EnsureInitializedAsync(ct);
        if (_channel is null) return;

        try
        {
            var routingKey = $"gateway.{eventType}.{serviceName}";
            var body = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(new
            {
                eventType,
                serviceName,
                detail,
                timestamp = DateTime.UtcNow
            });

            var props = new BasicProperties
            {
                ContentType = "application/json",
                DeliveryMode = DeliveryModes.Persistent,
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };

            await _channel.BasicPublishAsync(
                exchange: _settings.ExchangeName,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: props,
                body: body,
                cancellationToken: ct);

            _logger.LogDebug("Published gateway event {EventType} for {Service}", eventType, serviceName);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to publish gateway event {EventType}", eventType);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null) await _channel.CloseAsync();
        if (_connection is not null) await _connection.CloseAsync();
    }
}

public sealed class NoOpGatewayEventPublisher : IGatewayEventPublisher
{
    public Task PublishRouteEventAsync(string eventType, string serviceName, string detail, CancellationToken ct = default)
        => Task.CompletedTask;
}
