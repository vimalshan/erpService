using System.Text.Json;
using AttendanceService.Infrastructure.EventBus.RabbitMQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Exceptions;

namespace AttendanceService.Infrastructure.EventBus.RabbitMQ.Consumers;

public class SwipePunchConsumer(EventBusRabbitMQ eventBus,
    IOptions<RabbitMQSettings> settings,
    ILogger<SwipePunchConsumer> logger) : BackgroundService
{
    private static readonly TimeSpan[] RetryDelays =
        [TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(1)];

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var exchange = settings.Value.ExchangeName;
        var queue = settings.Value.QueueName;
        var routingKey = settings.Value.RoutingKey;

        logger.LogInformation("SwipePunchConsumer starting...");
        var attempt = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await eventBus.SubscribeAsync(queue, exchange, routingKey, HandleMessageAsync);
                logger.LogInformation("SwipePunchConsumer connected to RabbitMQ.");
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // Graceful shutdown — exit loop
                break;
            }
            catch (BrokerUnreachableException ex)
            {
                var delay = RetryDelays[Math.Min(attempt, RetryDelays.Length - 1)];
                attempt++;
                logger.LogWarning(ex,
                    "RabbitMQ unavailable (attempt {Attempt}). Retrying in {Delay}s...",
                    attempt, delay.TotalSeconds);
                await Task.Delay(delay, stoppingToken);
            }
            catch (Exception ex)
            {
                var delay = RetryDelays[Math.Min(attempt, RetryDelays.Length - 1)];
                attempt++;
                logger.LogError(ex,
                    "SwipePunchConsumer error (attempt {Attempt}). Retrying in {Delay}s...",
                    attempt, delay.TotalSeconds);
                await Task.Delay(delay, stoppingToken);
            }
        }
        logger.LogInformation("SwipePunchConsumer stopped.");
    }

    private async Task HandleMessageAsync(string message)
    {
        logger.LogInformation("Received swipe punch message: {Message}", message);
        // Parse and process domain logic
        var payload = JsonSerializer.Deserialize<Dictionary<string, object>>(message);
        if (payload is not null)
            logger.LogInformation("SwipePunchConsumer processed EmpId={EmpId}",
                payload.GetValueOrDefault("EmpSysId"));

        await Task.CompletedTask;
    }
}
