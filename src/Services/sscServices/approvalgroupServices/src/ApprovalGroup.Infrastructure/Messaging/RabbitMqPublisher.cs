using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ApprovalGroup.Domain.Interfaces;

namespace ApprovalGroup.Infrastructure.Messaging;

public class RabbitMqSettings
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public string ExchangeName { get; set; } = "approval_group_exchange";
}

public class RabbitMqPublisher : IMessagePublisher, IAsyncDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<RabbitMqPublisher> _logger;
    private bool _connectionFailed;

    public RabbitMqPublisher(IOptions<RabbitMqSettings> settings, ILogger<RabbitMqPublisher> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    private async Task EnsureConnectedAsync()
    {
        if (_channel is not null) return;
        if (_connectionFailed) return;

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

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            await _channel.ExchangeDeclareAsync(_settings.ExchangeName, ExchangeType.Topic, durable: true);
        }
        catch (Exception ex)
        {
            _connectionFailed = true;
            _logger.LogWarning(ex, "[RabbitMQ] Could not connect to broker at {Host}:{Port}. Publishing will be skipped.",
                _settings.HostName, _settings.Port);
        }
    }

    public async Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default)
    {
        await EnsureConnectedAsync();
        if (_channel is null)
        {
            _logger.LogWarning("[RabbitMQ] Skipping publish of {MessageType} to {RoutingKey} – broker unavailable",
                typeof(T).Name, routingKey);
            return;
        }

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        var props = new BasicProperties { Persistent = true, ContentType = "application/json" };

        await _channel.BasicPublishAsync(
            exchange: _settings.ExchangeName,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: props,
            body: body,
            cancellationToken: ct);

        _logger.LogInformation("Published message to {Exchange}/{RoutingKey}", _settings.ExchangeName, routingKey);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null) await _channel.CloseAsync();
        if (_connection is not null) await _connection.CloseAsync();
    }
}
