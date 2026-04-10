using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SSCTransactional.Infrastructure.Settings;

namespace SSCTransactional.Infrastructure.Messaging;

public class RabbitMQPublisher : IAsyncDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly ILogger<RabbitMQPublisher> _logger;
    private readonly RabbitMQSettings _settings;
    private bool _connectionFailed;

    public RabbitMQPublisher(IOptions<RabbitMQSettings> options, ILogger<RabbitMQPublisher> logger)
    {
        _settings = options.Value;
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
                HostName = _settings.Host,
                Port = _settings.Port,
                UserName = _settings.Username,
                Password = _settings.Password,
                VirtualHost = _settings.VirtualHost
            };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
        }
        catch (Exception ex)
        {
            _connectionFailed = true;
            _logger.LogWarning(ex, "[RabbitMQ] Could not connect to broker at {Host}:{Port}. Publishing will be skipped.", _settings.Host, _settings.Port);
        }
    }

    public async Task PublishAsync<T>(string exchangeName, string routingKey, T message)
    {
        await EnsureConnectedAsync();
        if (_channel is null)
        {
            _logger.LogWarning("[RabbitMQ] Skipping publish of {MessageType} – broker unavailable", typeof(T).Name);
            return;
        }

        await _channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct, durable: true);
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        var props = new BasicProperties { ContentType = "application/json", DeliveryMode = DeliveryModes.Persistent };
        await _channel.BasicPublishAsync(exchangeName, routingKey, mandatory: false, basicProperties: props, body: body);
        _logger.LogInformation("[RabbitMQ] Published {MessageType} to {Exchange}/{RoutingKey}", typeof(T).Name, exchangeName, routingKey);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null) await _channel.DisposeAsync();
        if (_connection is not null) await _connection.DisposeAsync();
    }
}
