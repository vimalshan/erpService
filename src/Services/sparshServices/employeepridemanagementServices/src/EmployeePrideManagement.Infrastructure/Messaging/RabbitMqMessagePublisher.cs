using System.Text;
using System.Text.Json;
using EmployeePrideManagement.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace EmployeePrideManagement.Infrastructure.Messaging;

public class RabbitMqMessagePublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly ILogger<RabbitMqMessagePublisher> _logger;
    private readonly ConnectionFactory _factory;
    private IConnection? _connection;
    private IChannel? _channel;
    private bool _isConnected;

    public RabbitMqMessagePublisher(IConfiguration configuration, ILogger<RabbitMqMessagePublisher> logger)
    {
        _logger = logger;

        _factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:HostName"] ?? "localhost",
            UserName = configuration["RabbitMQ:UserName"] ?? "guest",
            Password = configuration["RabbitMQ:Password"] ?? "guest",
            Port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672")
        };
    }

    private async Task EnsureConnectedAsync(CancellationToken cancellationToken)
    {
        if (_isConnected && _connection is { IsOpen: true } && _channel is { IsOpen: true })
            return;

        try
        {
            _connection = await _factory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);
            _isConnected = true;
        }
        catch (Exception ex)
        {
            _isConnected = false;
            _logger.LogWarning(ex, "Could not connect to RabbitMQ. Messages will be skipped.");
        }
    }

    public async Task PublishAsync<T>(string queueName, T message, CancellationToken cancellationToken = default) where T : class
    {
        await EnsureConnectedAsync(cancellationToken);

        if (!_isConnected || _channel is null)
        {
            _logger.LogWarning("RabbitMQ not available. Skipping publish to {QueueName}.", queueName);
            return;
        }

        await _channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = new BasicProperties
        {
            Persistent = true,
            ContentType = "application/json"
        };

        await _channel.BasicPublishAsync(
            exchange: string.Empty,
            routingKey: queueName,
            mandatory: false,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);

        _logger.LogInformation("Published message to queue {QueueName}", queueName);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is { IsOpen: true })
            await _channel.CloseAsync();
        if (_connection is { IsOpen: true })
            await _connection.CloseAsync();
    }
}
