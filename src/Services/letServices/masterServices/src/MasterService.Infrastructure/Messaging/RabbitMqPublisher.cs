using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace MasterService.Infrastructure.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync<T>(string exchange, string routingKey, T message);
}

public sealed class RabbitMqPublisher(IConfiguration configuration, ILogger<RabbitMqPublisher> logger)
    : IMessagePublisher, IDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;
    private bool _disposed;

    private async Task EnsureConnectionAsync()
    {
        if (_connection?.IsOpen == true && _channel?.IsOpen == true) return;

        _channel?.Dispose();
        _connection?.Dispose();

        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:Host"] ?? "localhost",
            Port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672"),
            UserName = configuration["RabbitMQ:Username"] ?? "guest",
            Password = configuration["RabbitMQ:Password"] ?? "guest",
            VirtualHost = configuration["RabbitMQ:VirtualHost"] ?? "/"
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        logger.LogInformation("RabbitMQ connection established.");
    }

    public async Task PublishAsync<T>(string exchange, string routingKey, T message)
    {
        await EnsureConnectionAsync();

        await _channel!.ExchangeDeclareAsync(
            exchange: exchange,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await _channel!.BasicPublishAsync(
            exchange: exchange,
            routingKey: routingKey,
            body: body);

        logger.LogInformation("Published message to exchange={Exchange}, routingKey={RoutingKey}", exchange, routingKey);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _channel?.Dispose();
        _connection?.Dispose();
        _disposed = true;
    }
}
