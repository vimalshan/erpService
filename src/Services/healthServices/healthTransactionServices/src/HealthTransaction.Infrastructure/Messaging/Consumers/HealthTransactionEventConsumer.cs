using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace HealthTransaction.Infrastructure.Messaging.Consumers;

/// <summary>
/// Background service that consumes cross-service events published to the
/// health.transaction.events exchange.
/// </summary>
public class HealthTransactionEventConsumer : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<HealthTransactionEventConsumer> _logger;
    private IConnection? _connection;
    private IChannel? _channel;

    private const string Exchange = "health.transaction.events";
    private const string Queue = "health.transaction.queue";
    private const string RoutingKey = "#";

    public HealthTransactionEventConsumer(
        IConfiguration configuration,
        ILogger<HealthTransactionEventConsumer> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
                Port = int.TryParse(_configuration["RabbitMQ:Port"], out var p) ? p : 5672,
                UserName = _configuration["RabbitMQ:Username"] ?? "guest",
                Password = _configuration["RabbitMQ:Password"] ?? "guest"
            };

            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.ExchangeDeclareAsync(
                exchange: Exchange,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                cancellationToken: stoppingToken);

            await _channel.QueueDeclareAsync(
                queue: Queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: stoppingToken);

            await _channel.QueueBindAsync(
                queue: Queue,
                exchange: Exchange,
                routingKey: RoutingKey,
                cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);
                    _logger.LogInformation(
                        "HealthTransactionEventConsumer received message on routing key {RoutingKey}: {Body}",
                        ea.RoutingKey, json);

                    // Dispatch to specific handler based on routing key
                    await HandleMessageAsync(ea.RoutingKey, json, stoppingToken);
                    await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message from queue {Queue}", Queue);
                    await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, cancellationToken: stoppingToken);
                }
            };

            await _channel.BasicConsumeAsync(
                queue: Queue,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);

            _logger.LogInformation("HealthTransactionEventConsumer started, listening on queue {Queue}", Queue);

            // Keep running until cancellation
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            // Graceful shutdown
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "HealthTransactionEventConsumer failed to start or encountered a fatal error. Consumer will not restart.");
        }
    }

    private Task HandleMessageAsync(string routingKey, string json, CancellationToken cancellationToken)
    {
        switch (routingKey)
        {
            case "preemployment.created":
                _logger.LogInformation("Processing preemployment.created event: {Json}", json);
                // TODO: add cross-service business logic (e.g., notify insurance service)
                break;

            case "checkupcard.created":
                _logger.LogInformation("Processing checkupcard.created event: {Json}", json);
                // TODO: add cross-service business logic
                break;

            default:
                _logger.LogDebug("No handler registered for routing key: {RoutingKey}", routingKey);
                break;
        }
        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);
        if (_channel is not null) await _channel.CloseAsync(cancellationToken);
        if (_connection is not null) await _connection.CloseAsync(cancellationToken);
    }
}
