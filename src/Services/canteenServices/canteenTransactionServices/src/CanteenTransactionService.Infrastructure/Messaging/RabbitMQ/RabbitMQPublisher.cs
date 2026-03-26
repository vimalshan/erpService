using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using CanteenTransactionService.Domain.Interfaces;

namespace CanteenTransactionService.Infrastructure.Messaging.RabbitMQ;

public class RabbitMQSettings
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string VirtualHost { get; set; } = "/";
    public string Username { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string ExchangeName { get; set; } = "canteentransaction.exchange";
}

public class RabbitMQPublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly RabbitMQSettings _settings;
    private readonly ILogger<RabbitMQPublisher> _logger;
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMQPublisher(IOptions<RabbitMQSettings> settings, ILogger<RabbitMQPublisher> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default) where T : class
    {
        await EnsureConnectionAsync(ct);
        if (_channel is null) return;

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var props = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent,
            MessageId = Guid.NewGuid().ToString(),
            Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
        };

        await _channel.BasicPublishAsync(
            exchange: _settings.ExchangeName,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: props,
            body: body,
            cancellationToken: ct);

        _logger.LogInformation("Published message to {Exchange}/{RoutingKey}", _settings.ExchangeName, routingKey);
    }

    private async Task EnsureConnectionAsync(CancellationToken ct)
    {
        if (_connection is { IsOpen: true } && _channel is { IsOpen: true }) return;

        var factory = new ConnectionFactory
        {
            HostName = _settings.Host,
            Port = _settings.Port,
            VirtualHost = _settings.VirtualHost,
            UserName = _settings.Username,
            Password = _settings.Password
        };

        _connection = await factory.CreateConnectionAsync(ct);
        _channel = await _connection.CreateChannelAsync(cancellationToken: ct);

        await _channel.ExchangeDeclareAsync(
            exchange: _settings.ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            cancellationToken: ct);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null) await _channel.CloseAsync();
        if (_connection is not null) await _connection.CloseAsync();
    }
}
