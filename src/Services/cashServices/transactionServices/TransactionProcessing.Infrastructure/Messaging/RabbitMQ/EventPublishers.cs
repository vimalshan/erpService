using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using TransactionProcessing.Domain.Interfaces;
using TransactionProcessing.Infrastructure.Messaging.Settings;

namespace TransactionProcessing.Infrastructure.Messaging.RabbitMQ;

public sealed class RabbitMqEventPublisher : IEventPublisher, IAsyncDisposable
{
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<RabbitMqEventPublisher> _logger;
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqEventPublisher(IOptions<RabbitMqSettings> settings, ILogger<RabbitMqEventPublisher> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task PublishAsync<T>(T @event, string routingKey, CancellationToken ct = default) where T : class
    {
        try
        {
            await EnsureConnectionAsync(ct);
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));
            var props = new BasicProperties
            {
                ContentType = "application/json",
                DeliveryMode = DeliveryModes.Persistent,
                MessageId = Guid.NewGuid().ToString(),
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };
            await _channel!.BasicPublishAsync(_settings.ExchangeName, routingKey, false, props, body, ct);
            _logger.LogInformation("Published {EventType} to {RoutingKey}", typeof(T).Name, routingKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish {EventType} to {RoutingKey}", typeof(T).Name, routingKey);
            throw;
        }
    }

    private async Task EnsureConnectionAsync(CancellationToken ct)
    {
        if (_connection is { IsOpen: true } && _channel is { IsOpen: true }) return;

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
        await _channel.ExchangeDeclareAsync(_settings.ExchangeName, "topic", true, false, cancellationToken: ct);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null) await _channel.CloseAsync();
        if (_connection is not null) await _connection.CloseAsync();
    }
}

public sealed class NoOpEventPublisher(ILogger<NoOpEventPublisher> logger) : IEventPublisher
{
    public Task PublishAsync<T>(T @event, string routingKey, CancellationToken ct = default) where T : class
    {
        logger.LogInformation("[NoOp] Would publish {EventType} to {RoutingKey}", typeof(T).Name, routingKey);
        return Task.CompletedTask;
    }
}
