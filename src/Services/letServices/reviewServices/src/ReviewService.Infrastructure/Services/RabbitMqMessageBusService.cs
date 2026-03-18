using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using ReviewService.Domain.Interfaces;

namespace ReviewService.Infrastructure.Services;

public class RabbitMqMessageBusService : IMessageBusService, IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly ILogger<RabbitMqMessageBusService> _logger;

    private RabbitMqMessageBusService(IConnection connection, IChannel channel, ILogger<RabbitMqMessageBusService> logger)
    {
        _connection = connection;
        _channel = channel;
        _logger = logger;
    }

    public static async Task<RabbitMqMessageBusService> CreateAsync(
        IConnectionFactory factory, ILogger<RabbitMqMessageBusService> logger)
    {
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();
        return new RabbitMqMessageBusService(connection, channel, logger);
    }

    public async Task PublishAsync<T>(
        string exchange, string routingKey, T message,
        CancellationToken cancellationToken = default) where T : class
    {
        await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: cancellationToken);
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        var props = new BasicProperties { ContentType = "application/json", DeliveryMode = DeliveryModes.Persistent };
        await _channel.BasicPublishAsync(exchange, routingKey, false, props, body, cancellationToken);
        _logger.LogInformation("Published message to {Exchange}/{RoutingKey}", exchange, routingKey);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }
}
