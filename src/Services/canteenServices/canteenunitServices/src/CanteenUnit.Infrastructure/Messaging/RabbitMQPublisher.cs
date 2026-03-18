using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace CanteenUnit.Infrastructure.Messaging;

public class RabbitMQPublisher : IRabbitMQPublisher, IAsyncDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly IConfiguration _config;
    private readonly ILogger<RabbitMQPublisher> _logger;
    private bool _initialized;

    public RabbitMQPublisher(IConfiguration config, ILogger<RabbitMQPublisher> logger)
    {
        _config = config;
        _logger = logger;
    }

    private async Task EnsureInitializedAsync()
    {
        if (_initialized) return;
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _config["RabbitMQ:Host"] ?? "localhost",
                Port = int.Parse(_config["RabbitMQ:Port"] ?? "5672"),
                UserName = _config["RabbitMQ:Username"] ?? "guest",
                Password = _config["RabbitMQ:Password"] ?? "guest",
                VirtualHost = _config["RabbitMQ:VHost"] ?? "/"
            };
            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            _initialized = true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "RabbitMQ not available — publisher will be inactive");
        }
    }

    public async Task PublishAsync<T>(T message, string exchange, string routingKey, CancellationToken ct = default)
    {
        await EnsureInitializedAsync();
        if (_channel is null) return;

        try
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);
            var props = new BasicProperties { ContentType = "application/json", Persistent = true };
            await _channel.BasicPublishAsync(exchange, routingKey, mandatory: false,
                basicProperties: props, body: body, cancellationToken: ct);
            _logger.LogDebug("Published {Type} to {Exchange}/{RoutingKey}", typeof(T).Name, exchange, routingKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish message to RabbitMQ");
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null) await _channel.DisposeAsync();
        if (_connection is not null) await _connection.DisposeAsync();
    }
}
