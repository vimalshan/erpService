using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace LocationServices.Infrastructure.Messaging;

// ── OPTIONS ──────────────────────────────────────────────────────────────────
public sealed class RabbitMQOptions
{
    public string HostName   { get; init; } = "localhost";
    public int    Port       { get; init; } = 5672;
    public string UserName   { get; init; } = "guest";
    public string Password   { get; init; } = "guest";
    public string Exchange   { get; init; } = "location.exchange";
    public string Queue      { get; init; } = "location.events";
    public string RoutingKey { get; init; } = "location.#";
}

// ── PUBLISHER (Adapter pattern) ───────────────────────────────────────────────
public interface IMessagePublisher
{
    Task PublishAsync<T>(string routingKey, T message, CancellationToken ct = default);
}

/// <summary>
/// RabbitMQ publisher using client v7 async API.
/// Lazily initialises the connection on first publish — avoids startup failure
/// when RabbitMQ is unavailable in dev/test environments.
/// </summary>
public sealed class RabbitMQPublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly RabbitMQOptions _opts;
    private readonly ILogger<RabbitMQPublisher> _logger;
    private IConnection? _connection;
    private IChannel?    _channel;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public RabbitMQPublisher(IOptions<RabbitMQOptions> options, ILogger<RabbitMQPublisher> logger)
    {
        _opts   = options.Value;
        _logger = logger;
    }

    public async Task PublishAsync<T>(string routingKey, T message, CancellationToken ct = default)
    {
        await EnsureChannelAsync(ct);
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        var props = new BasicProperties
        {
            Persistent  = true,
            ContentType = "application/json",
            MessageId   = Guid.NewGuid().ToString(),
            Timestamp   = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
        };
        await _channel!.BasicPublishAsync(_opts.Exchange, routingKey, false, props, body, ct);
        _logger.LogDebug("[RabbitMQ] Published {RoutingKey}: {MessageId}", routingKey, props.MessageId);
    }

    private async Task EnsureChannelAsync(CancellationToken ct)
    {
        if (_channel is { IsOpen: true }) return;
        await _lock.WaitAsync(ct);
        try
        {
            if (_channel is { IsOpen: true }) return;
            var factory = new ConnectionFactory
            {
                HostName = _opts.HostName, Port = _opts.Port,
                UserName = _opts.UserName, Password = _opts.Password,
                AutomaticRecoveryEnabled = true
            };
            _connection = await factory.CreateConnectionAsync(ct);
            _channel    = await _connection.CreateChannelAsync(cancellationToken: ct);
            await _channel.ExchangeDeclareAsync(_opts.Exchange, ExchangeType.Topic, durable: true, cancellationToken: ct);
            await _channel.QueueDeclareAsync(_opts.Queue, durable: true, exclusive: false, autoDelete: false, cancellationToken: ct);
            await _channel.QueueBindAsync(_opts.Queue, _opts.Exchange, _opts.RoutingKey, cancellationToken: ct);
        }
        finally { _lock.Release(); }
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel  is not null) await _channel.CloseAsync();
        if (_connection is not null) await _connection.CloseAsync();
        _lock.Dispose();
    }
}

// ── CONSUMER ─────────────────────────────────────────────────────────────────
public sealed class RabbitMQConsumer : IAsyncDisposable
{
    private readonly RabbitMQOptions _opts;
    private readonly ILogger<RabbitMQConsumer> _logger;
    private IConnection? _connection;
    private IChannel?    _channel;

    public RabbitMQConsumer(IOptions<RabbitMQOptions> options, ILogger<RabbitMQConsumer> logger)
    {
        _opts   = options.Value;
        _logger = logger;
    }

    public async Task StartConsumingAsync(string queue, Func<string, Task> handler, CancellationToken ct = default)
    {
        var factory = new ConnectionFactory
        {
            HostName = _opts.HostName, Port = _opts.Port,
            UserName = _opts.UserName, Password = _opts.Password,
            AutomaticRecoveryEnabled = true
        };
        _connection = await factory.CreateConnectionAsync(ct);
        _channel    = await _connection.CreateChannelAsync(cancellationToken: ct);
        await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 10, global: false, ct);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body = Encoding.UTF8.GetString(ea.Body.ToArray());
            try
            {
                await handler(body);
                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                _logger.LogDebug("[RabbitMQ] ACK {DeliveryTag}", ea.DeliveryTag);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RabbitMQ] NACK {DeliveryTag}", ea.DeliveryTag);
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
            }
        };
        await _channel.BasicConsumeAsync(queue, autoAck: false, consumer: consumer, cancellationToken: ct);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel    is not null) await _channel.CloseAsync();
        if (_connection is not null) await _connection.CloseAsync();
    }
}
