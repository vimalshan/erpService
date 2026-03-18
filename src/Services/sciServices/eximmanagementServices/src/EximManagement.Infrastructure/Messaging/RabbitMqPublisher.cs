using EximManagement.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace EximManagement.Infrastructure.Messaging;

public class RabbitMqPublisher(IConfiguration configuration, ILogger<RabbitMqPublisher> logger) 
    : IMessagePublisher, IDisposable
{
    private readonly string _hostName = configuration["RabbitMQ:Host"] ?? "localhost";
    private readonly string _userName = configuration["RabbitMQ:Username"] ?? "guest";
    private readonly string _password = configuration["RabbitMQ:Password"] ?? "guest";
    private readonly string _exchange = configuration["RabbitMQ:Exchange"] ?? "exim.exchange";
    private IConnection? _connection;
    private IChannel? _channel;

    private async Task EnsureChannelAsync()
    {
        if (_channel is { IsOpen: true }) return;

        var factory = new ConnectionFactory
        {
            HostName = _hostName,
            UserName = _userName,
            Password = _password
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        await _channel.ExchangeDeclareAsync(_exchange, ExchangeType.Topic, durable: true, autoDelete: false);
    }

    public async Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default) where T : class
    {
        try
        {
            await EnsureChannelAsync();

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var props = new BasicProperties
            {
                ContentType = "application/json",
                DeliveryMode = DeliveryModes.Persistent,
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };

            await _channel!.BasicPublishAsync(_exchange, routingKey, false, props, body, ct);
            logger.LogInformation("Published message to {Exchange} with key {RoutingKey}", _exchange, routingKey);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to publish message to RabbitMQ: {RoutingKey}", routingKey);
            throw;
        }
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        GC.SuppressFinalize(this);
    }
}
