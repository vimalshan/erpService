using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrganizationSetup.Application.Interfaces;
using RabbitMQ.Client;

namespace OrganizationSetup.Infrastructure.Messaging;

public class RabbitMqMessagePublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<RabbitMqMessagePublisher> _logger;
    private IConnection? _connection;
    private IChannel? _channel;
    private bool _initialized;

    public RabbitMqMessagePublisher(IOptions<RabbitMqSettings> options, ILogger<RabbitMqMessagePublisher> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }

    private async Task EnsureInitializedAsync()
    {
        if (_initialized) return;

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

        await _channel.ExchangeDeclareAsync(
            exchange: _settings.ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false);

        _initialized = true;
        _logger.LogInformation("RabbitMQ connection established to {Host}:{Port}", _settings.HostName, _settings.Port);
    }

    public async Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default)
    {
        try
        {
            await EnsureInitializedAsync();

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            await _channel!.BasicPublishAsync(
                exchange: exchange,
                routingKey: routingKey,
                body: body,
                cancellationToken: ct);

            _logger.LogInformation("Published to {Exchange}/{RoutingKey}: {MessageType}",
                exchange, routingKey, typeof(T).Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish message to {Exchange}/{RoutingKey}. Message will be lost.", exchange, routingKey);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null) await _channel.DisposeAsync();
        if (_connection is not null) await _connection.DisposeAsync();
    }
}
