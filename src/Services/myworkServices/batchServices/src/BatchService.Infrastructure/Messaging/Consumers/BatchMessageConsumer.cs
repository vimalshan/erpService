using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BatchService.Infrastructure.Messaging.Consumers;

/// <summary>Background service that consumes batch.events messages from RabbitMQ.</summary>
public sealed class BatchMessageConsumer : BackgroundService
{
    private readonly IConfiguration _config;
    private readonly ILogger<BatchMessageConsumer> _logger;
    private IConnection? _connection;
    private IChannel?    _channel;

    public BatchMessageConsumer(IConfiguration config, ILogger<BatchMessageConsumer> logger)
    {
        _config = config;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await ConnectAndConsumeAsync(stoppingToken);
        }
        catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogWarning(ex, "[RabbitMQ Consumer] Could not connect to RabbitMQ. Consumer is disabled.");
        }
    }

    private async Task ConnectAndConsumeAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName    = _config["RabbitMQ:Host"]        ?? "localhost",
            Port        = int.TryParse(_config["RabbitMQ:Port"], out var p) ? p : 5672,
            UserName    = _config["RabbitMQ:Username"]    ?? "guest",
            Password    = _config["RabbitMQ:Password"]    ?? "guest",
            VirtualHost = _config["RabbitMQ:VirtualHost"] ?? "/"
        };

        var exchange = _config["RabbitMQ:Exchange"] ?? "batch.events";
        var queue    = _config["RabbitMQ:ConsumerQueue"] ?? "batch.consumer.queue";

        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel    = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
        await _channel.QueueDeclareAsync(queue, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await _channel.QueueBindAsync(queue, exchange, "BatchCreatedEvent",       cancellationToken: stoppingToken);
        await _channel.QueueBindAsync(queue, exchange, "BatchStatusChangedEvent", cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                _logger.LogInformation("[RabbitMQ Consumer] Received: {RoutingKey} | Body: {Body}", ea.RoutingKey, body);

                // TODO: Deserialize and dispatch to MediatR handlers as needed
                await _channel!.BasicAckAsync(ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RabbitMQ Consumer] Error processing message");
                await _channel!.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
            }
        };

        await _channel.BasicConsumeAsync(queue, autoAck: false, consumer, stoppingToken);

        // Keep alive until cancellation
        await Task.Delay(Timeout.Infinite, stoppingToken).ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null) await _channel.CloseAsync(cancellationToken);
        if (_connection is not null) await _connection.CloseAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}
