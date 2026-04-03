using System.Text;
using System.Text.Json;
using LoanTransaction.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace LoanTransaction.Infrastructure.Messaging;

public class RabbitMQMessageBus : IMessageBus, IAsyncDisposable
{
    private readonly RabbitMQSettings _settings;
    private readonly ILogger<RabbitMQMessageBus> _logger;
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly SemaphoreSlim _lock = new(1, 1);

    public RabbitMQMessageBus(IOptions<RabbitMQSettings> options, ILogger<RabbitMQMessageBus> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }

    private async Task EnsureConnectedAsync()
    {
        if (_connection is { IsOpen: true } && _channel is { IsOpen: true })
            return;

        await _lock.WaitAsync();
        try
        {
            if (_connection is { IsOpen: true } && _channel is { IsOpen: true })
                return;

            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            await _channel.ExchangeDeclareAsync(
                _settings.ExchangeName, ExchangeType.Topic, durable: true);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default) where T : class
    {
        try
        {
            await EnsureConnectedAsync();

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            await _channel!.BasicPublishAsync(
                exchange: _settings.ExchangeName,
                routingKey: routingKey,
                body: body,
                cancellationToken: ct);

            _logger.LogInformation("Published message {Type} with routing key {Key}",
                typeof(T).Name, routingKey);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex,
                "Failed to publish message {Type}. RabbitMQ may be offline.", typeof(T).Name);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null) await _channel.DisposeAsync();
        if (_connection is not null) await _connection.DisposeAsync();
        _lock.Dispose();
    }
}
