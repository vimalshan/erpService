using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ArchiveService.Infrastructure.Messaging;

public abstract class RabbitMqConsumerBase(
    IConnection connection,
    ILogger logger,
    string queueName,
    string exchange,
    string routingKey) : BackgroundService
{
    private IChannel? _channel;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
        await _channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await _channel.QueueBindAsync(queueName, exchange, routingKey, cancellationToken: stoppingToken);
        await _channel.BasicQosAsync(0, 1, false, stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                await HandleMessageAsync(body, stoppingToken);
                await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing message from queue {Queue}", queueName);
                await _channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);
            }
        };

        await _channel.BasicConsumeAsync(queueName, autoAck: false, consumer, stoppingToken);
    }

    protected abstract Task HandleMessageAsync(string message, CancellationToken ct);

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null)
            await _channel.CloseAsync(cancellationToken);
        await base.StopAsync(cancellationToken);
    }
}

public class ArchiveOrderConsumer(IConnection connection, ILogger<ArchiveOrderConsumer> logger)
    : RabbitMqConsumerBase(connection, logger, "archive-order-queue", "archive-exchange", "order.archived")
{
    protected override Task HandleMessageAsync(string message, CancellationToken ct)
    {
        logger.LogInformation("Received archive order message: {Message}", message);
        return Task.CompletedTask;
    }
}

public class ArchivePurgeConsumer(IConnection connection, ILogger<ArchivePurgeConsumer> logger)
    : RabbitMqConsumerBase(connection, logger, "archive-purge-queue", "archive-exchange", "archive.purge")
{
    protected override Task HandleMessageAsync(string message, CancellationToken ct)
    {
        logger.LogInformation("Received purge request: {Message}", message);
        return Task.CompletedTask;
    }
}
