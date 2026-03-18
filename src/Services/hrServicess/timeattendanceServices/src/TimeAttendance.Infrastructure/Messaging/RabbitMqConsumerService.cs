using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TimeAttendance.Infrastructure.Messaging;

/// <summary>
/// Background service that listens on RabbitMQ queues and processes inbound messages.
/// </summary>
public class RabbitMqConsumerService(
    IOptions<RabbitMqOptions> options,
    IServiceScopeFactory scopeFactory,
    ILogger<RabbitMqConsumerService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Retry loop: keep trying to connect; don't crash the host if RabbitMQ is unavailable
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ConnectAndConsumeAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex,
                    "RabbitMQ consumer disconnected. Reconnecting in 10 seconds...");
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken).ConfigureAwait(false);
            }
        }
    }

    private async Task ConnectAndConsumeAsync(CancellationToken stoppingToken)
    {
        var opt = options.Value;
        var factory = new ConnectionFactory
        {
            HostName = opt.Host,
            Port = opt.Port,
            UserName = opt.UserName,
            Password = opt.Password,
            VirtualHost = opt.VirtualHost
        };

        await using var connection = await factory.CreateConnectionAsync(stoppingToken);
        await using var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.ExchangeDeclareAsync(opt.ExchangeName, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);

        var queueName = "timeattendance.worker";
        await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await channel.QueueBindAsync(queueName, opt.ExchangeName, "timeattendance.#", cancellationToken: stoppingToken);

        logger.LogInformation("RabbitMQ consumer connected and listening on queue '{Queue}'.", queueName);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                var routingKey = ea.RoutingKey;
                logger.LogInformation("Received message on '{RoutingKey}': {Body}", routingKey, body);

                await using var scope = scopeFactory.CreateAsyncScope();
                var handlers = scope.ServiceProvider.GetServices<IMessageHandler>();
                foreach (var handler in handlers.Where(h => h.CanHandle(routingKey)))
                    await handler.HandleAsync(routingKey, body, stoppingToken);

                await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing RabbitMQ message");
                await channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true, stoppingToken);
            }
        };

        await channel.BasicConsumeAsync(queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}

public interface IMessageHandler
{
    bool CanHandle(string routingKey);
    Task HandleAsync(string routingKey, string body, CancellationToken cancellationToken);
}

/// <summary>
/// Handles absenteeism created messages.
/// </summary>
public class AbsenteeismCreatedMessageHandler(ILogger<AbsenteeismCreatedMessageHandler> logger)
    : IMessageHandler
{
    public bool CanHandle(string routingKey)
        => routingKey.StartsWith("timeattendance.absenteeism.created", StringComparison.OrdinalIgnoreCase);

    public Task HandleAsync(string routingKey, string body, CancellationToken cancellationToken)
    {
        var payload = JsonSerializer.Deserialize<JsonElement>(body);
        logger.LogInformation("Processing absenteeism created event: {Payload}", payload);
        // Additional processing (e.g., trigger downstream notifications) goes here
        return Task.CompletedTask;
    }
}
