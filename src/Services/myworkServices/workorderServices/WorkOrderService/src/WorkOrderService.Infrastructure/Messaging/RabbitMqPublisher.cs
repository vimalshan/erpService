using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using WorkOrderService.Application.Interfaces;

namespace WorkOrderService.Infrastructure.Messaging;

public class RabbitMqPublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly ConnectionFactory _factory;
    private readonly ILogger<RabbitMqPublisher> _logger;
    private IConnection? _connection;
    private IChannel? _channel;
    private bool _unavailable;

    public RabbitMqPublisher(ConnectionFactory factory, ILogger<RabbitMqPublisher> logger)
    {
        _factory = factory;
        _logger = logger;
    }

    private async Task<IChannel?> GetChannelAsync(CancellationToken cancellationToken)
    {
        if (_unavailable) return null;
        if (_channel is not null) return _channel;

        try
        {
            _connection = await _factory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
            return _channel;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "RabbitMQ is unavailable. Messages will not be published.");
            _unavailable = true;
            return null;
        }
    }

    public async Task PublishAsync<T>(string queueName, T message, CancellationToken cancellationToken = default) where T : class
    {
        var channel = await GetChannelAsync(cancellationToken);
        if (channel is null)
        {
            _logger.LogWarning("Skipping publish to {Queue} — RabbitMQ not connected.", queueName);
            return;
        }

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var props = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent,
            ContentType = "application/json"
        };

        await channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            mandatory: false,
            basicProperties: props,
            body: body,
            cancellationToken: cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null) await _channel.CloseAsync();
        if (_connection is not null) await _connection.CloseAsync();
        GC.SuppressFinalize(this);
    }
}
