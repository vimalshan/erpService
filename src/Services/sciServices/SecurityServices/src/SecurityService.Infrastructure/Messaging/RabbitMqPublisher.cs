using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SecurityService.Application.Interfaces;

namespace SecurityService.Infrastructure.Messaging;

public sealed class RabbitMqOptions
{
    public const string Section = "RabbitMQ";
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
}

public sealed class RabbitMqPublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly ILogger<RabbitMqPublisher> _logger;

    public RabbitMqPublisher(IOptions<RabbitMqOptions> opts, ILogger<RabbitMqPublisher> logger)
    {
        _logger = logger;
        var factory = new ConnectionFactory
        {
            HostName = opts.Value.HostName,
            Port = opts.Value.Port,
            UserName = opts.Value.UserName,
            Password = opts.Value.Password,
            VirtualHost = opts.Value.VirtualHost
        };
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
    }

    public async Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default) where T : class
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: cancellationToken);
        await _channel.BasicPublishAsync(
            exchange: exchange,
            routingKey: routingKey,
            body: body,
            cancellationToken: cancellationToken);

        _logger.LogInformation("Published message to {Exchange}/{RoutingKey}: {Message}", exchange, routingKey, json);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.DisposeAsync();
        await _connection.DisposeAsync();
    }
}

/// <summary>Consumer for security domain events from RabbitMQ.</summary>
public sealed class SecurityEventConsumer : IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly ILogger<SecurityEventConsumer> _logger;

    public SecurityEventConsumer(IOptions<RabbitMqOptions> opts, ILogger<SecurityEventConsumer> logger)
    {
        _logger = logger;
        var factory = new ConnectionFactory
        {
            HostName = opts.Value.HostName,
            Port = opts.Value.Port,
            UserName = opts.Value.UserName,
            Password = opts.Value.Password,
            VirtualHost = opts.Value.VirtualHost
        };
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
    }

    public async Task StartConsumingAsync(string queueName, CancellationToken cancellationToken)
    {
        await _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += (_, ea) =>
        {
            var body = Encoding.UTF8.GetString(ea.Body.ToArray());
            _logger.LogInformation("Received message from {Queue}: {Body}", queueName, body);
            return _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken: default).AsTask();
        };
        await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer, cancellationToken: cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
