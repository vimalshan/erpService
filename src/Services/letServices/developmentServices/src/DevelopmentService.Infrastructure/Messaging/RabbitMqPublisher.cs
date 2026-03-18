using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using DevelopmentService.Domain.Interfaces;

namespace DevelopmentService.Infrastructure.Messaging;

public class RabbitMqPublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly ILogger<RabbitMqPublisher> _logger;

    private RabbitMqPublisher(IConnection connection, IChannel channel, ILogger<RabbitMqPublisher> logger)
    {
        _connection = connection;
        _channel    = channel;
        _logger     = logger;
    }

    public static async Task<RabbitMqPublisher> CreateAsync(
        IConfiguration configuration,
        ILogger<RabbitMqPublisher> logger)
    {
        var rabbitConfig = configuration.GetSection("RabbitMQ");
        var factory = new ConnectionFactory
        {
            HostName    = rabbitConfig["Host"] ?? "localhost",
            Port        = int.TryParse(rabbitConfig["Port"], out var port) ? port : 5672,
            UserName    = rabbitConfig["Username"] ?? "guest",
            Password    = rabbitConfig["Password"] ?? "guest",
            VirtualHost = rabbitConfig["VirtualHost"] ?? "/"
        };

        var connection = await factory.CreateConnectionAsync();
        var channel    = await connection.CreateChannelAsync();
        return new RabbitMqPublisher(connection, channel, logger);
    }

    public async Task PublishAsync<T>(
        string exchange, string routingKey, T message, CancellationToken ct = default)
    {
        try
        {
            await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: ct);
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var props = new BasicProperties { Persistent = true, ContentType = "application/json" };
            await _channel.BasicPublishAsync(exchange, routingKey, false, props, body, ct);
            _logger.LogInformation("Published message to {Exchange}/{RoutingKey}", exchange, routingKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish message to {Exchange}/{RoutingKey}", exchange, routingKey);
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
        _channel.Dispose();
        _connection.Dispose();
    }
}
