using System.Text;
using System.Text.Json;
using EnergyService.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace EnergyService.Infrastructure.Services;

public class RabbitMqPublisher : IRabbitMqPublisher, IDisposable
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly ILogger<RabbitMqPublisher> _logger;
    private IConnection? _connection;
    private IChannel? _channel;
    private bool _connected;

    public RabbitMqPublisher(IConnectionFactory connectionFactory, ILogger<RabbitMqPublisher> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    private async Task EnsureConnectedAsync(CancellationToken ct)
    {
        if (_connected && _connection is not null && _channel is not null) return;

        try
        {
            _connection = await _connectionFactory.CreateConnectionAsync(ct);
            _channel = await _connection.CreateChannelAsync(cancellationToken: ct);
            _connected = true;
        }
        catch (Exception ex)
        {
            _connected = false;
            _logger.LogWarning(ex, "RabbitMQ is not available. Messages will be skipped.");
        }
    }

    public async Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default)
    {
        await EnsureConnectedAsync(ct);
        if (!_connected || _channel is null)
        {
            _logger.LogWarning("RabbitMQ not connected. Skipping publish to {Exchange}/{RoutingKey}", exchange, routingKey);
            return;
        }

        await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: ct);
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        var props = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent
        };

        await _channel.BasicPublishAsync(exchange, routingKey, mandatory: false, basicProperties: props, body: body, cancellationToken: ct);
        _logger.LogInformation("Published message to {Exchange}/{RoutingKey}", exchange, routingKey);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
