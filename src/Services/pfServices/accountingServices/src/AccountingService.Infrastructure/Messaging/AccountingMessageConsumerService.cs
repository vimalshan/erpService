using AccountingService.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AccountingService.Infrastructure.Messaging;

/// <summary>Background hosted service that consumes the accounting.transactions queue.</summary>
public class AccountingMessageConsumerService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AccountingMessageConsumerService> _logger;

    public AccountingMessageConsumerService(
        IServiceScopeFactory scopeFactory,
        ILogger<AccountingMessageConsumerService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Accounting Message Consumer Service starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await using var scope = _scopeFactory.CreateAsyncScope();
                var consumer = scope.ServiceProvider.GetRequiredService<RabbitMqConsumer>();

                await consumer.StartConsumingAsync("accounting.transactions", async message =>
                {
                    _logger.LogInformation("Processing accounting message: {Message}", message);
                    await Task.CompletedTask;
                }, stoppingToken);

                // Keep service alive until cancellation
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Normal shutdown — do not retry
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "RabbitMQ consumer encountered an error. Retrying in 30 seconds...");

                try { await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); }
                catch (OperationCanceledException) { break; }
            }
        }

        _logger.LogInformation("Accounting Message Consumer Service stopped.");
    }
}
