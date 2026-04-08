using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using SciTransactional.Application.Interfaces;

namespace SciTransactional.Infrastructure.Services;

public sealed class RabbitMqPublisher : IRabbitMqPublisher, IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly ILogger<RabbitMqPublisher> _logger;

    private RabbitMqPublisher(IConnection connection, IChannel channel,
        ILogger<RabbitMqPublisher> logger)
    {
        _connection = connection;
        _channel = channel;
        _logger = logger;
    }

    public static async Task<RabbitMqPublisher> CreateAsync(
        IConfiguration configuration, ILogger<RabbitMqPublisher> logger)
    {
        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:HostName"] ?? "localhost",
            UserName = configuration["RabbitMQ:UserName"] ?? "guest",
            Password = configuration["RabbitMQ:Password"] ?? "guest"
        };

        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();
        return new RabbitMqPublisher(connection, channel, logger);
    }

    public async Task PublishAsync<T>(string exchange, string routingKey, T message,
        CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic,
            durable: true, cancellationToken: ct);

        var props = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent
        };

        await _channel.BasicPublishAsync(exchange, routingKey, mandatory: false,
            basicProperties: props, body: body, cancellationToken: ct);

        _logger.LogInformation("Published message to {Exchange}/{RoutingKey}", exchange, routingKey);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }
}

public sealed class NoOpRabbitMqPublisher : IRabbitMqPublisher
{
    public Task PublishAsync<T>(string exchange, string routingKey, T message,
        CancellationToken ct = default) => Task.CompletedTask;
}
