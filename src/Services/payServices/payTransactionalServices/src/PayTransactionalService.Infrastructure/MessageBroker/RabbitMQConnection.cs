using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PayTransactionalService.Infrastructure.MessageBroker;

public interface IMessageBrokerConnection
{
    bool IsConnected { get; }
    Task ConnectAsync();
    Task DisconnectAsync();
    Task PublishAsync(string exchange, string routingKey, object message, CancellationToken ct = default);
}

public class RabbitMQConnection : IMessageBrokerConnection, IAsyncDisposable
{
    private readonly ILogger<RabbitMQConnection> _logger;
    private readonly string _hostName;
    private readonly string _userName;
    private readonly string _password;
    private readonly int _port;
    private readonly string _virtualHost;
    private IConnection? _connection;
    private IChannel? _channel;

    public bool IsConnected => _connection?.IsOpen == true && _channel?.IsOpen == true;

    public RabbitMQConnection(ILogger<RabbitMQConnection> logger, string hostName, string userName, string password, int port = 5672, string virtualHost = "/")
    {
        _logger = logger; _hostName = hostName; _userName = userName; _password = password; _port = port; _virtualHost = virtualHost;
    }

    public async Task ConnectAsync()
    {
        try
        {
            _logger.LogInformation("Connecting to RabbitMQ at {Host}:{Port}", _hostName, _port);
            var factory = new ConnectionFactory
            {
                HostName = _hostName, UserName = _userName, Password = _password,
                Port = _port, VirtualHost = _virtualHost,
                RequestedConnectionTimeout = TimeSpan.FromSeconds(5)
            };
            _connection = await factory.CreateConnectionAsync("PayTransactionalService");
            _channel = await _connection.CreateChannelAsync();
            _logger.LogInformation("Connected to RabbitMQ at {Host}:{Port}", _hostName, _port);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Cannot connect to RabbitMQ at {Host}:{Port} — service will run without messaging", _hostName, _port);
        }
    }

    public async Task DisconnectAsync()
    {
        try
        {
            if (_channel?.IsOpen == true) await _channel.CloseAsync();
            if (_connection?.IsOpen == true) await _connection.CloseAsync();
            _logger.LogInformation("Disconnected from RabbitMQ");
        }
        catch (Exception ex) { _logger.LogError(ex, "Error disconnecting from RabbitMQ"); }
    }

    public async Task PublishAsync(string exchange, string routingKey, object message, CancellationToken ct = default)
    {
        if (!IsConnected) { _logger.LogWarning("RabbitMQ not connected — skipping publish"); return; }
        try
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);
            await _channel!.BasicPublishAsync(exchange, routingKey, body, ct);
            _logger.LogDebug("Published to {Exchange}/{RoutingKey}", exchange, routingKey);
        }
        catch (Exception ex) { _logger.LogError(ex, "Failed to publish to {Exchange}/{RoutingKey}", exchange, routingKey); }
    }

    public async ValueTask DisposeAsync()
    {
        await DisconnectAsync();
        _channel?.Dispose();
        _connection?.Dispose();
    }
}

public interface IMessageConsumer
{
    void StartConsuming(string queueName);
    void StopConsuming();
}

public class PayTransactionMessageConsumer : IMessageConsumer
{
    private readonly ILogger<PayTransactionMessageConsumer> _logger;
    private readonly IMessageBrokerConnection _connection;

    public PayTransactionMessageConsumer(ILogger<PayTransactionMessageConsumer> logger, IMessageBrokerConnection connection)
    { _logger = logger; _connection = connection; }

    public void StartConsuming(string queueName)
    {
        if (!_connection.IsConnected)
        { _logger.LogWarning("RabbitMQ not connected — consumer not started for {Queue}", queueName); return; }
        _logger.LogInformation("Consumer started for queue: {Queue}", queueName);
    }

    public void StopConsuming() => _logger.LogInformation("Consumer stopped");
}
