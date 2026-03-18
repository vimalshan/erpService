using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace GSTComplianceService.Infrastructure.Messaging;

public abstract class RabbitMqConsumerBase : BackgroundService
{
    protected readonly ILogger Logger;
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly IConfiguration _configuration;
    protected abstract string QueueName { get; }
    protected abstract string ExchangeName { get; }
    protected abstract string RoutingKey { get; }

    protected RabbitMqConsumerBase(IConfiguration configuration, ILogger logger)
    {
        _configuration = configuration;
        Logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:Host"] ?? "localhost",
            Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
            UserName = _configuration["RabbitMQ:Username"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest",
        };

        _connection = await factory.CreateConnectionAsync(stoppingToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
        await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await _channel.QueueBindAsync(QueueName, ExchangeName, RoutingKey, cancellationToken: stoppingToken);
        await _channel.BasicQosAsync(0, 1, false, stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                await ProcessMessageAsync(body, stoppingToken);
                await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error processing message from queue {QueueName}", QueueName);
                await _channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);
            }
        };

        await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer, stoppingToken);
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    protected abstract Task ProcessMessageAsync(string message, CancellationToken cancellationToken);

    public override void Dispose()
    {
        _channel?.DisposeAsync().AsTask().Wait();
        _connection?.DisposeAsync().AsTask().Wait();
        base.Dispose();
    }
}

// ── GST Registered Consumer ───────────────────────────────────────
public class GstRegisteredConsumer : RabbitMqConsumerBase
{
    protected override string QueueName => "gst.registered";
    protected override string ExchangeName => "gst.events";
    protected override string RoutingKey => "gst.registered";

    public GstRegisteredConsumer(IConfiguration configuration, ILogger<GstRegisteredConsumer> logger)
        : base(configuration, logger) { }

    protected override Task ProcessMessageAsync(string message, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Processing GstRegistered event: {Message}", message);
        // TODO: Integrate with downstream services (e.g., send welcome email, notify analytics)
        return Task.CompletedTask;
    }
}

// ── GST Status Changed Consumer ───────────────────────────────────
public class GstStatusChangedConsumer : RabbitMqConsumerBase
{
    protected override string QueueName => "gst.status-changed";
    protected override string ExchangeName => "gst.events";
    protected override string RoutingKey => "gst.status.#";

    public GstStatusChangedConsumer(IConfiguration configuration, ILogger<GstStatusChangedConsumer> logger)
        : base(configuration, logger) { }

    protected override Task ProcessMessageAsync(string message, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Processing GstStatusChanged event: {Message}", message);
        // TODO: Trigger downstream workflows based on status changes
        return Task.CompletedTask;
    }
}
