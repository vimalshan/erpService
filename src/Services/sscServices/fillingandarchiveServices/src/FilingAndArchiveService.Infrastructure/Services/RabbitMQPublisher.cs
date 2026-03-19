using System.Text;
using System.Text.Json;
using FilingAndArchiveService.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace FilingAndArchiveService.Infrastructure.Services;

public class RabbitMQPublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly ILogger<RabbitMQPublisher> _logger;

    private RabbitMQPublisher(IConnection connection, IChannel channel, ILogger<RabbitMQPublisher> logger)
    {
        _connection = connection;
        _channel = channel;
        _logger = logger;
    }

    public static async Task<RabbitMQPublisher> CreateAsync(
        string hostName,
        string userName,
        string password,
        int port,
        ILogger<RabbitMQPublisher> logger)
    {
        var factory = new ConnectionFactory
        {
            HostName = hostName,
            UserName = userName,
            Password = password,
            Port = port
        };

        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();
        return new RabbitMQPublisher(connection, channel, logger);
    }

    public async Task PublishAsync<T>(
        string exchange,
        string routingKey,
        T message,
        CancellationToken cancellationToken = default) where T : class
    {
        await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: cancellationToken);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var props = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent
        };

        await _channel.BasicPublishAsync(exchange, routingKey, false, props, body, cancellationToken);
        _logger.LogInformation("Published message to exchange={Exchange} routingKey={RoutingKey}", exchange, routingKey);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
