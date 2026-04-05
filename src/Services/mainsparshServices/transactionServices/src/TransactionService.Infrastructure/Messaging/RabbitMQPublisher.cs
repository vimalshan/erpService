using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Infrastructure.Messaging;

public class RabbitMQPublisher : IMessagePublisher, IDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly ILogger<RabbitMQPublisher> _logger;
    private readonly ConnectionFactory _factory;
    private bool _unavailable;

    public RabbitMQPublisher(IConfiguration configuration, ILogger<RabbitMQPublisher> logger)
    {
        _logger = logger;
        _factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:Host"] ?? "localhost",
            Port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672"),
            UserName = configuration["RabbitMQ:Username"] ?? "guest",
            Password = configuration["RabbitMQ:Password"] ?? "guest",
            VirtualHost = configuration["RabbitMQ:VirtualHost"] ?? "/"
        };
    }

    private async Task EnsureConnectedAsync(CancellationToken cancellationToken)
    {
        if (_unavailable || (_connection?.IsOpen == true && _channel?.IsOpen == true))
            return;

        try
        {
            _connection = await _factory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _unavailable = true;
            _logger.LogWarning(ex, "RabbitMQ is unavailable. Messages will not be published.");
        }
    }

    public async Task PublishAsync<T>(T message, string queueName, CancellationToken cancellationToken = default) where T : class
    {
        await EnsureConnectedAsync(cancellationToken);

        if (_unavailable || _channel is null)
        {
            _logger.LogWarning("Skipping publish — RabbitMQ is unavailable. queue={QueueName}", queueName);
            return;
        }

        try
        {
            await _channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = new BasicProperties
            {
                ContentType = "application/json",
                DeliveryMode = DeliveryModes.Persistent,
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };

            await _channel.BasicPublishAsync("", queueName, true, properties, body, cancellationToken);
            _logger.LogInformation("Published message to queue={QueueName}", queueName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish message to queue={QueueName}", queueName);
            throw;
        }
    }

    public void Dispose()
    {
        _channel?.CloseAsync().GetAwaiter().GetResult();
        _connection?.CloseAsync().GetAwaiter().GetResult();
    }
}
