using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BusServices.Infrastructure.Messaging.RabbitMQ;

public sealed class RabbitMQSettings
{
    public string Host { get; init; } = "localhost";
    public int Port { get; init; } = 5672;
    public string Username { get; init; } = "guest";
    public string Password { get; init; } = "guest";
    public string VirtualHost { get; init; } = "/";
}

public interface IMessagePublisher
{
    Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default);
}

public sealed class RabbitMQPublisher : IMessagePublisher, IDisposable
{
    private readonly IConnection? _connection;
    private readonly IChannel? _channel;
    private readonly ILogger<RabbitMQPublisher> _logger;
    private readonly bool _isAvailable;

    public RabbitMQPublisher(IOptions<RabbitMQSettings> options, ILogger<RabbitMQPublisher> logger)
    {
        _logger = logger;
        var cfg = options.Value;
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = cfg.Host,
                Port = cfg.Port,
                UserName = cfg.Username,
                Password = cfg.Password,
                VirtualHost = cfg.VirtualHost
            };
            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
            _isAvailable = true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "RabbitMQ unavailable — event publishing will be skipped.");
            _isAvailable = false;
        }
    }

    public async Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default)
    {
        if (!_isAvailable)
        {
            _logger.LogDebug("RabbitMQ unavailable — skipping publish to {Exchange}/{RoutingKey}", exchange, routingKey);
            return;
        }

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        await _channel!.BasicPublishAsync(exchange, routingKey, body, ct);
        _logger.LogInformation("Published message to exchange={Exchange}, key={RoutingKey}", exchange, routingKey);
    }

    public void Dispose()
    {
        _channel?.CloseAsync().GetAwaiter().GetResult();
        _connection?.CloseAsync().GetAwaiter().GetResult();
    }
}
