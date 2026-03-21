using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ReceivingService.Infrastructure.MessageBroker.RabbitMQ;

/// <summary>
/// Background service that consumes receiving-related messages from RabbitMQ.
/// Each consumed message is dispatched to a typed handler via the service locator.
/// </summary>
public sealed class ReceivingMessageConsumer : BackgroundService
{
    private readonly RabbitMQSettings _settings;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ReceivingMessageConsumer> _logger;

    private IConnection? _connection;
    private IChannel? _channel;

    public const string QueueName    = "receiving.events";
    public const string ExchangeName = "receiving.exchange";
    public const string RoutingKey   = "receiving.#";

    public ReceivingMessageConsumer(
        IOptions<RabbitMQSettings> settings,
        IServiceScopeFactory scopeFactory,
        ILogger<ReceivingMessageConsumer> logger)
    {
        _settings     = settings.Value;
        _scopeFactory = scopeFactory;
        _logger       = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var retryDelays = new[] { 5, 10, 20, 30, 60 };
        int attempt = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _settings.Host,
                    Port     = _settings.Port,
                    UserName = _settings.UserName,
                    Password = _settings.Password
                };

                _connection = await factory.CreateConnectionAsync(stoppingToken);
                _channel    = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                await _channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Topic, durable: true,
                    cancellationToken: stoppingToken);
                await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false,
                    autoDelete: false, cancellationToken: stoppingToken);
                await _channel.QueueBindAsync(QueueName, ExchangeName, RoutingKey,
                    cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += HandleMessageAsync;

                await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer,
                    cancellationToken: stoppingToken);

                _logger.LogInformation("RabbitMQ consumer started on queue '{Queue}'", QueueName);
                attempt = 0; // reset on successful connection

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // Normal shutdown — exit the loop cleanly
                break;
            }
            catch (Exception ex)
            {
                var delaySecs = retryDelays[Math.Min(attempt, retryDelays.Length - 1)];
                _logger.LogWarning(ex,
                    "RabbitMQ consumer connection failed (attempt {Attempt}). " +
                    "Retrying in {Delay}s. Start RabbitMQ to enable messaging.",
                    ++attempt, delaySecs);

                // Dispose stale connection/channel before retrying
                if (_channel is not null)    { await _channel.DisposeAsync();    _channel    = null; }
                if (_connection is not null) { await _connection.DisposeAsync(); _connection = null; }

                try { await Task.Delay(TimeSpan.FromSeconds(delaySecs), stoppingToken); }
                catch (OperationCanceledException) { break; }
            }
        }
    }

    private async Task HandleMessageAsync(object sender, BasicDeliverEventArgs ea)
    {
        var body    = Encoding.UTF8.GetString(ea.Body.Span);
        var routing = ea.RoutingKey;

        _logger.LogInformation("Received RabbitMQ message. RoutingKey={RoutingKey}", routing);

        try
        {
            using var scope   = _scopeFactory.CreateScope();
            // Extend here: deserialise by routing key and dispatch to a process manager
            await Task.CompletedTask;

            await _channel!.BasicAckAsync(ea.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing RabbitMQ message. RoutingKey={RoutingKey}", routing);
            await _channel!.BasicNackAsync(ea.DeliveryTag, false, requeue: true);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null) await _channel.DisposeAsync();
        if (_connection is not null) await _connection.DisposeAsync();
        await base.StopAsync(cancellationToken);
    }
}
