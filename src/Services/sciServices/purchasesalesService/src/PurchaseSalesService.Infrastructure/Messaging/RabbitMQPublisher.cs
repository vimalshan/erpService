using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using PurchaseSalesService.Application.Common.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PurchaseSalesService.Infrastructure.Messaging;

public sealed class RabbitMQPublisher : IMessagePublisher, IDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly IConfiguration _configuration;
    private readonly ILogger<RabbitMQPublisher> _logger;
    private readonly AsyncCircuitBreakerPolicy _circuitBreaker;
    private readonly SemaphoreSlim _initLock = new(1, 1);
    private const string ExchangeName = "purchase_sales_exchange";

    public RabbitMQPublisher(IConfiguration configuration, ILogger<RabbitMQPublisher> logger)
    {
        _configuration = configuration;
        _logger = logger;

        _circuitBreaker = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (ex, ts) => _logger.LogWarning("RabbitMQ circuit opened for {Duration}s. Reason: {Ex}", ts.TotalSeconds, ex.Message),
                onReset: () => _logger.LogInformation("RabbitMQ circuit reset."));
    }

    private async Task EnsureConnectedAsync(CancellationToken ct)
    {
        if (_channel is not null) return;
        await _initLock.WaitAsync(ct);
        try
        {
            if (_channel is not null) return;
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:Host"] ?? "localhost",
                Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
                UserName = _configuration["RabbitMQ:Username"] ?? "guest",
                Password = _configuration["RabbitMQ:Password"] ?? "guest",
                VirtualHost = _configuration["RabbitMQ:VirtualHost"] ?? "/"
            };
            _connection = await factory.CreateConnectionAsync(ct);
            _channel = await _connection.CreateChannelAsync(cancellationToken: ct);
            await _channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Topic, durable: true, cancellationToken: ct);
        }
        finally
        {
            _initLock.Release();
        }
    }

    public async Task PublishAsync<T>(string routingKey, T message, CancellationToken ct = default)
    {
        await _circuitBreaker.ExecuteAsync(async () =>
        {
            await EnsureConnectedAsync(ct);
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var props = new BasicProperties { Persistent = true, ContentType = "application/json" };
            await _channel!.BasicPublishAsync(ExchangeName, routingKey, false, props, body, ct);
            _logger.LogInformation("Published message to {RoutingKey}", routingKey);
        });
    }

    public void Dispose()
    {
        _channel?.CloseAsync().GetAwaiter().GetResult();
        _connection?.CloseAsync().GetAwaiter().GetResult();
    }
}
