using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LoanManagement.Infrastructure.Messaging;

public class LoanEventConsumer : BackgroundService
{
    private readonly RabbitMqPublisher _publisher;
    private readonly ILogger<LoanEventConsumer> _logger;

    public LoanEventConsumer(RabbitMqPublisher publisher, ILogger<LoanEventConsumer> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("LoanEventConsumer starting...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _publisher.SubscribeAsync<LoanCreatedMessage>(
                    queue: "loan.created",
                    exchange: "loan.events",
                    routingKey: "loan.created",
                    handler: async msg =>
                    {
                        _logger.LogInformation("Received LoanCreated: LoanId={LoanId}", msg.LoanId);
                        // Implement downstream processing here
                        await Task.CompletedTask;
                    });

                // Keep alive until cancelled or connection drops
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break; // Graceful shutdown — do not retry
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "RabbitMQ unavailable — retrying in 30 s...");
                try { await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); }
                catch (OperationCanceledException) { break; }
            }
        }

        _logger.LogInformation("LoanEventConsumer stopped.");
    }
}

public record LoanCreatedMessage(decimal LoanId, string LoanKey, decimal Amount);
