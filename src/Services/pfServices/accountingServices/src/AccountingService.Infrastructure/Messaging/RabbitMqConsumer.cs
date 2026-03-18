using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace AccountingService.Infrastructure.Messaging;

public class RabbitMqConsumer : IAsyncDisposable, IDisposable
{
    private readonly ILogger<RabbitMqConsumer> _logger;
    private readonly AsyncCircuitBreakerPolicy _circuitBreaker;
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqConsumer(IConfiguration configuration, ILogger<RabbitMqConsumer> logger)
    {
        _logger = logger;

        _circuitBreaker = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (ex, ts) => _logger.LogWarning("RabbitMQ circuit breaker OPEN for {Duration}s. Reason: {Error}", ts.TotalSeconds, ex.Message),
                onReset: () => _logger.LogInformation("RabbitMQ circuit breaker CLOSED."),
                onHalfOpen: () => _logger.LogInformation("RabbitMQ circuit breaker HALF-OPEN."));
    }

    public async Task StartConsumingAsync(string queueName, Func<string, Task> messageHandler, CancellationToken ct = default)
    {
        await _circuitBreaker.ExecuteAsync(async () =>
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = await factory.CreateConnectionAsync(ct);
            _channel = await _connection.CreateChannelAsync(cancellationToken: ct);

            await _channel.QueueDeclareAsync(queueName, durable: true, exclusive: false,
                autoDelete: false, arguments: null, cancellationToken: ct);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _logger.LogInformation("Received message on queue {Queue}: {Message}", queueName, message);
                    await messageHandler(message);
                    await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, ct);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message from queue {Queue}", queueName);
                    await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true, ct);
                }
            };

            await _channel.BasicConsumeAsync(queueName, autoAck: false, consumer: consumer, cancellationToken: ct);
            _logger.LogInformation("Started consuming from queue: {Queue}", queueName);
        });
    }

    public async Task PublishAsync<T>(string queueName, T message, CancellationToken ct = default)
    {
        await _circuitBreaker.ExecuteAsync(async () =>
        {
            if (_channel is null) throw new InvalidOperationException("Channel not initialised.");

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            await _channel.BasicPublishAsync(
                exchange: string.Empty, routingKey: queueName,
                mandatory: false, body: body, cancellationToken: ct);

            _logger.LogInformation("Published message to queue: {Queue}", queueName);
        });
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null) await _channel.DisposeAsync();
        if (_connection is not null) await _connection.DisposeAsync();
    }

    public void Dispose()
    {
        (_channel as IDisposable)?.Dispose();
        (_connection as IDisposable)?.Dispose();
    }
}
