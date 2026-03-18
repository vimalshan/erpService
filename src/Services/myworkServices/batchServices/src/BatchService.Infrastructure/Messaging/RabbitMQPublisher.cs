using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using BatchService.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BatchService.Infrastructure.Messaging;

public sealed class RabbitMQPublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel    _channel;
    private readonly string      _exchange;
    private readonly ILogger<RabbitMQPublisher> _logger;

    private RabbitMQPublisher(IConnection connection, IChannel channel, string exchange, ILogger<RabbitMQPublisher> logger)
    {
        _connection = connection;
        _channel    = channel;
        _exchange   = exchange;
        _logger     = logger;
    }

    public static async Task<RabbitMQPublisher> CreateAsync(IConfiguration config, ILogger<RabbitMQPublisher> logger)
    {
        var factory = new ConnectionFactory
        {
            HostName = config["RabbitMQ:Host"] ?? "localhost",
            Port     = int.TryParse(config["RabbitMQ:Port"], out var p) ? p : 5672,
            UserName = config["RabbitMQ:Username"] ?? "guest",
            Password = config["RabbitMQ:Password"] ?? "guest",
            VirtualHost = config["RabbitMQ:VirtualHost"] ?? "/"
        };

        var exchange = config["RabbitMQ:Exchange"] ?? "batch.events";
        var connection = await factory.CreateConnectionAsync();
        var channel    = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true);
        return new RabbitMQPublisher(connection, channel, exchange, logger);
    }

    public async Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default) where T : class
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var props = new BasicProperties
        {
            ContentType  = "application/json",
            DeliveryMode = DeliveryModes.Persistent,
            MessageId    = Guid.NewGuid().ToString()
        };

        await _channel.BasicPublishAsync(_exchange, routingKey, false, props, body, ct);
        _logger.LogInformation("[RabbitMQ] Published {RoutingKey}", routingKey);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }
}
