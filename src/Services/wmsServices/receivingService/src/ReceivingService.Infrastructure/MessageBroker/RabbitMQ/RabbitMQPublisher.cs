using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using ReceivingService.Infrastructure.MessageBroker;

namespace ReceivingService.Infrastructure.MessageBroker.RabbitMQ;

public sealed class RabbitMQPublisher : IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly ILogger<RabbitMQPublisher> _logger;

    private RabbitMQPublisher(
        IConnection connection, IChannel channel,
        ILogger<RabbitMQPublisher> logger)
    {
        _connection = connection;
        _channel    = channel;
        _logger     = logger;
    }

    public static async Task<RabbitMQPublisher> CreateAsync(
        RabbitMQSettings settings,
        ILogger<RabbitMQPublisher> logger)
    {
        var factory = new ConnectionFactory
        {
            HostName = settings.Host,
            Port     = settings.Port,
            UserName = settings.UserName,
            Password = settings.Password
        };
        var connection = await factory.CreateConnectionAsync();
        var channel    = await connection.CreateChannelAsync();
        return new RabbitMQPublisher(connection, channel, logger);
    }

    public async Task PublishAsync<T>(string exchange, string routingKey, T message)
    {
        await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true);
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        var props = new BasicProperties { ContentType = "application/json", DeliveryMode = DeliveryModes.Persistent };
        await _channel.BasicPublishAsync(exchange, routingKey, false, props, body);
        _logger.LogInformation("Published {Type} to {Exchange}/{RoutingKey}", typeof(T).Name, exchange, routingKey);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
