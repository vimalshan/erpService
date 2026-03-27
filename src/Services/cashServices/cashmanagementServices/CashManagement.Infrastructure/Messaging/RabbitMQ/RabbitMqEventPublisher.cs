using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using CashManagement.Domain.Interfaces;
using CashManagement.Infrastructure.Messaging.Settings;

namespace CashManagement.Infrastructure.Messaging.RabbitMQ;

public class RabbitMqEventPublisher : IEventPublisher, IAsyncDisposable
{
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<RabbitMqEventPublisher> _logger;
    private IConnection? _connection;
    private IChannel? _channel;
    private bool _connected;

    public RabbitMqEventPublisher(IOptions<RabbitMqSettings> options, ILogger<RabbitMqEventPublisher> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }

    public async Task PublishAsync<T>(string routingKey, T message)
    {
        try
        {
            if (!_connected)
                await ConnectAsync();

            if (_channel is null) return;

            var exchange = _settings.ExchangeName ?? "cashmanagement";
            await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            await _channel.BasicPublishAsync(exchange: exchange, routingKey: routingKey, body: body);
            _logger.LogInformation("Published event to RabbitMQ — Exchange: {Exchange}, Routing: {RoutingKey}", exchange, routingKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "RabbitMQ unavailable — event {RoutingKey} not published. The application continues normally.", routingKey);
        }
    }

    private async Task ConnectAsync()
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost
            };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            _connected = true;
            _logger.LogInformation("Connected to RabbitMQ at {Host}:{Port}", _settings.Host, _settings.Port);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to connect to RabbitMQ at {Host}:{Port}. Events will not be published.", _settings.Host, _settings.Port);
            _connected = false;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null) await _channel.DisposeAsync();
        if (_connection is not null) await _connection.DisposeAsync();
    }
}
