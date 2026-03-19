using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using MenuAndSecurityService.Infrastructure.Messaging;
using System.Text;

namespace MenuAndSecurityService.Functions.Workers;

public class AuditLogWorker : BackgroundService
{
    private readonly RabbitMqConnection _connection;
    private readonly ILogger<AuditLogWorker> _logger;

    public AuditLogWorker(RabbitMqConnection connection, ILogger<AuditLogWorker> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("AuditLogWorker started - listening for all menu events");

        try
        {
            var channel = await _connection.GetChannelAsync();
            await channel.ExchangeDeclareAsync("menu-exchange", ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
            var queueResult = await channel.QueueDeclareAsync("audit-log-queue", durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
            await channel.QueueBindAsync(queueResult.QueueName, "menu-exchange", "menu.#", cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogInformation("Audit log entry - RoutingKey: {RoutingKey}, Timestamp: {Timestamp}, Message: {Message}",
                        ea.RoutingKey, DateTime.UtcNow, body);

                    // In production, write to persistent audit storage
                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing audit log message");
                    await channel.BasicNackAsync(ea.DeliveryTag, false, true);
                }
            };

            await channel.BasicConsumeAsync(queueResult.QueueName, false, consumer, stoppingToken);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("AuditLogWorker stopping");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AuditLogWorker error");
        }
    }
}
