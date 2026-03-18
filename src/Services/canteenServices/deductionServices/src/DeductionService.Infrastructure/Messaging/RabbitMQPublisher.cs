using System.Text;
using System.Text.Json;
using DeductionService.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace DeductionService.Infrastructure.Messaging;

public sealed class RabbitMQPublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly string _exchangeName;
    private readonly ILogger<RabbitMQPublisher> _logger;

    private RabbitMQPublisher(IConnection connection, IChannel channel, string exchangeName, ILogger<RabbitMQPublisher> logger)
    {
        _connection = connection;
        _channel = channel;
        _exchangeName = exchangeName;
        _logger = logger;
    }

    public static async Task<RabbitMQPublisher> CreateAsync(IConfiguration configuration, ILogger<RabbitMQPublisher> logger)
    {
        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:Host"] ?? "localhost",
            Port = int.TryParse(configuration["RabbitMQ:Port"], out var port) ? port : 5672,
            UserName = configuration["RabbitMQ:Username"] ?? "guest",
            Password = configuration["RabbitMQ:Password"] ?? "guest",
            VirtualHost = configuration["RabbitMQ:VirtualHost"] ?? "/"
        };

        var exchangeName = configuration["RabbitMQ:Exchange"] ?? "deduction.exchange";

        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Topic, durable: true, autoDelete: false);

        return new RabbitMQPublisher(connection, channel, exchangeName, logger);
    }

    public async Task PublishAsync<T>(string routingKey, T message, CancellationToken ct = default) where T : class
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var props = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent,
            Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
        };

        await _channel.BasicPublishAsync(_exchangeName, routingKey, false, props, body, ct);
        _logger.LogInformation("[RabbitMQ] Published {RoutingKey}: {Message}", routingKey, json);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
