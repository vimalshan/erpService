using System.Text;
using System.Text.Json;
using MemberService.Domain.Common;
using MemberService.Infrastructure.Messaging.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace MemberService.Infrastructure.Messaging;

public interface IRabbitMqPublisher
{
    Task PublishAsync<T>(T message, string exchange, string routingKey, CancellationToken ct = default)
        where T : class;
}

public class RabbitMqPublisher : IRabbitMqPublisher, IAsyncDisposable
{
    private readonly ConnectionFactory _factory;
    private readonly ILogger<RabbitMqPublisher> _logger;
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public RabbitMqPublisher(IConfiguration configuration, ILogger<RabbitMqPublisher> logger)
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

    private async Task EnsureConnectedAsync(CancellationToken ct)
    {
        if (_channel?.IsOpen == true) return;
        await _semaphore.WaitAsync(ct);
        try
        {
            if (_channel?.IsOpen == true) return;
            _connection = await _factory.CreateConnectionAsync(ct);
            _channel = await _connection.CreateChannelAsync(cancellationToken: ct);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task PublishAsync<T>(T message, string exchange, string routingKey, CancellationToken ct = default)
        where T : class
    {
        await EnsureConnectedAsync(ct);
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        await _channel!.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: ct);
        await _channel.BasicPublishAsync(exchange, routingKey, body, ct);
        _logger.LogInformation("Published {MessageType} to exchange {Exchange}", typeof(T).Name, exchange);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null) await _channel.DisposeAsync();
        if (_connection != null) await _connection.DisposeAsync();
        _semaphore.Dispose();
    }
}
