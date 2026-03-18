using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ItemMasterService.Domain.Interfaces;

namespace ItemMasterService.Infrastructure.Messaging.RabbitMQ;

public class RabbitMQSettings
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string VirtualHost { get; set; } = "/";
    public string Username { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string ExchangeName { get; set; } = "itemmaster.exchange";
}

public class RabbitMQPublisher : IMessagePublisher, IAsyncDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly RabbitMQSettings _settings;
    private readonly ILogger<RabbitMQPublisher> _logger;
    private bool _available;

    public RabbitMQPublisher(IOptions<RabbitMQSettings> settings, ILogger<RabbitMQPublisher> logger)
    {
        _settings = settings.Value;
        _logger = logger;

        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                VirtualHost = _settings.VirtualHost,
                UserName = _settings.Username,
                Password = _settings.Password
            };

            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

            _channel.ExchangeDeclareAsync(
                _settings.ExchangeName,
                ExchangeType.Topic,
                durable: true,
                autoDelete: false).GetAwaiter().GetResult();

            _available = true;
            _logger.LogInformation("[RabbitMQ] Connected to {Host}:{Port}", _settings.Host, _settings.Port);
        }
        catch (Exception ex)
        {
            _available = false;
            _logger.LogWarning(ex, "[RabbitMQ] Could not connect to broker at {Host}:{Port}. Publishing will be skipped until reconnected.", _settings.Host, _settings.Port);
        }
    }

    public async Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default) where T : class
    {
        if (!_available || _channel is null)
        {
            _logger.LogWarning("[RabbitMQ] Broker unavailable — skipping publish to {RoutingKey}", routingKey);
            return;
        }

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

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

        _logger.LogInformation("[RabbitMQ] Published message to {Exchange}/{RoutingKey}", _settings.ExchangeName, routingKey);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
