using System.Text;
using System.Text.Json;
using MedicineManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace MedicineManagement.Infrastructure.Messaging;

public class RabbitMqPublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly string _hostName;
    private readonly string _userName;
    private readonly string _password;
    private readonly ILogger<RabbitMqPublisher> _logger;
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public RabbitMqPublisher(string hostName, string userName, string password, ILogger<RabbitMqPublisher> logger)
    {
        _hostName = hostName;
        _userName = userName;
        _password = password;
        _logger = logger;
    }

    private async Task EnsureConnectedAsync(CancellationToken ct)
    {
        if (_channel is not null) return;
        await _lock.WaitAsync(ct);
        try
        {
            if (_channel is not null) return;
            var factory = new ConnectionFactory { HostName = _hostName, UserName = _userName, Password = _password };
            _connection = await factory.CreateConnectionAsync(ct);
            _channel = await _connection.CreateChannelAsync(cancellationToken: ct);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default)
    {
        try
        {
            await EnsureConnectedAsync(ct);
            await _channel!.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: ct);
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var props = new BasicProperties { Persistent = true };
            await _channel.BasicPublishAsync(exchange, routingKey, false, props, body, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to publish message to {Exchange}/{RoutingKey}. RabbitMQ may be unavailable.", exchange, routingKey);
            _channel = null;
            _connection = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null) await _channel.CloseAsync();
        if (_connection is not null) await _connection.CloseAsync();
        _lock.Dispose();
        GC.SuppressFinalize(this);
    }
}
