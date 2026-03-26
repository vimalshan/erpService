using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ReferenceService.Infrastructure.RabbitMQ;

/// <summary>
/// Publishes domain events to RabbitMQ using the async RabbitMQ.Client 7.x API.
/// </summary>
public sealed class RabbitMQPublisher : IAsyncDisposable
{
    private readonly RabbitMQConnectionFactory _factory;
    private readonly ILogger<RabbitMQPublisher> _logger;
    private IChannel? _channel;

    public RabbitMQPublisher(RabbitMQConnectionFactory factory, ILogger<RabbitMQPublisher> logger)
    {
        _factory = factory;
        _logger  = logger;
    }

    private async Task<IChannel> GetChannelAsync(CancellationToken ct)
    {
        if (_channel is null || _channel.IsClosed)
        {
            var conn = await _factory.GetConnectionAsync(ct);
            _channel = await conn.CreateChannelAsync(cancellationToken: ct);
        }
        return _channel;
    }

    public async Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default)
    {
        try
        {
            var channel = await GetChannelAsync(ct);
            await channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: ct);

            var body  = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var props = new BasicProperties { ContentType = "application/json", DeliveryMode = DeliveryModes.Persistent };

            await channel.BasicPublishAsync(exchange, routingKey, false, props, body, ct);
            _logger.LogInformation("Published to {Exchange}/{RoutingKey}", exchange, routingKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish to {Exchange}/{RoutingKey}", exchange, routingKey);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null) await _channel.CloseAsync();
        await _factory.DisposeAsync();
    }
}

/// <summary>
/// Hosted service that keeps consumers alive and reconnects with exponential backoff.
/// Extend the consumers list here as domain events are added to ReferenceService.
/// </summary>
public sealed class RabbitMQConsumerHostedService : BackgroundService
{
    private readonly RabbitMQConnectionFactory _factory;
    private readonly RabbitMQConfiguration _config;
    private readonly ILogger<RabbitMQConsumerHostedService> _logger;

    private static readonly int[] RetryDelaysSeconds = [5, 10, 20, 30, 60];

    public RabbitMQConsumerHostedService(
        RabbitMQConnectionFactory factory,
        RabbitMQConfiguration config,
        ILogger<RabbitMQConsumerHostedService> logger)
    {
        _factory = factory;
        _config  = config;
        _logger  = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Reference RabbitMQ Consumer Hosted Service starting");

        int attempt = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            IChannel? channel = null;
            try
            {
                var conn = await _factory.GetConnectionAsync(stoppingToken);
                channel = await conn.CreateChannelAsync(cancellationToken: stoppingToken);

                await channel.ExchangeDeclareAsync(_config.ExchangeName, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);

                // ── Queue: lovtype updates ────────────────────────────────────
                await channel.QueueDeclareAsync(_config.LovTypeQueue, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
                await channel.QueueBindAsync(_config.LovTypeQueue, _config.ExchangeName, "reference.lovtype.*", cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    var body    = Encoding.UTF8.GetString(ea.Body.Span);
                    var routing = ea.RoutingKey;
                    _logger.LogInformation("[ReferenceService] Received message on {RoutingKey}: {Body}", routing, body);

                    try   { await channel.BasicAckAsync(ea.DeliveryTag, false); }
                    catch (Exception ex) { _logger.LogError(ex, "Failed to ACK message {Tag}", ea.DeliveryTag); }
                };

                await channel.BasicConsumeAsync(_config.LovTypeQueue, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

                attempt = 0;
                _logger.LogInformation("Reference RabbitMQ consumers ready");
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Reference RabbitMQ Consumer Hosted Service stopping");
                break;
            }
            catch (Exception ex)
            {
                int delaySecs = attempt < RetryDelaysSeconds.Length
                    ? RetryDelaysSeconds[attempt]
                    : RetryDelaysSeconds[^1];

                _logger.LogError(ex,
                    "Reference RabbitMQ consumers failed. Retrying in {Delay}s (attempt {Attempt})",
                    delaySecs, attempt + 1);
                attempt++;

                if (channel is not null) await channel.CloseAsync();

                try   { await Task.Delay(TimeSpan.FromSeconds(delaySecs), stoppingToken); }
                catch (OperationCanceledException) { break; }
            }
            finally
            {
                if (channel is not null) { await channel.CloseAsync(); channel.Dispose(); }
            }
        }
    }
}
