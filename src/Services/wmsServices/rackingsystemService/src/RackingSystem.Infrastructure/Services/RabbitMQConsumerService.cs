using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RackingSystem.Infrastructure.Settings;

namespace RackingSystem.Infrastructure.Services;

/// <summary>Background service that consumes RabbitMQ messages for the Racking System.</summary>
public sealed class RabbitMQConsumerService : BackgroundService
{
    private readonly RabbitMQSettings _settings;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<RabbitMQConsumerService> _logger;
    private IConnection? _connection;
    private IChannel? _channel;

    private const string Exchange   = "racking.exchange";
    private const string Queue      = "racking.queue";
    private const string RoutingKey = "bin.status.changed";

    public RabbitMQConsumerService(IOptions<RabbitMQSettings> settings,
        IServiceScopeFactory scopeFactory, ILogger<RabbitMQConsumerService> logger)
    {
        _settings     = settings.Value;
        _scopeFactory = scopeFactory;
        _logger       = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Retry connecting to RabbitMQ with exponential back-off so a missing broker
        // doesn't crash the host — it will keep retrying in the background.
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ConnectAndConsumeAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break; // Graceful shutdown
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "RabbitMQ consumer failed. Retrying in 30 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken).ConfigureAwait(false);
            }
        }
    }

    private async Task ConnectAndConsumeAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName    = _settings.Host,
            Port        = _settings.Port,
            UserName    = _settings.Username,
            Password    = _settings.Password,
            VirtualHost = _settings.VirtualHost
        };

        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel    = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.ExchangeDeclareAsync(Exchange, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
        await _channel.QueueDeclareAsync(Queue, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await _channel.QueueBindAsync(Queue, Exchange, RoutingKey, cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var body    = ea.Body.ToArray();
            var payload = Encoding.UTF8.GetString(body);

            _logger.LogInformation("RabbitMQ message received on {Queue}: {Payload}", Queue, payload);

            try
            {
                var evt = JsonSerializer.Deserialize<BinStatusChangedMessage>(payload);
                if (evt != null)
                    _logger.LogInformation("Bin {BinId} status changed: {From} -> {To}",
                        evt.BinId, evt.PreviousStatus, evt.NewStatus);

                await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing RabbitMQ message");
                await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false, stoppingToken);
            }
        };

        await _channel.BasicConsumeAsync(Queue, autoAck: false, consumer, stoppingToken);

        // Keep running until cancellation or disconnection
        await Task.Delay(Timeout.Infinite, stoppingToken).ConfigureAwait(false);
    }

    public override async Task StopAsync(CancellationToken ct)
    {
        await base.StopAsync(ct);
        if (_channel != null) await _channel.DisposeAsync();
        if (_connection != null) await _connection.DisposeAsync();
    }

    private sealed record BinStatusChangedMessage(int BinId, string PreviousStatus, string NewStatus, DateTime Timestamp);
}
