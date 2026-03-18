using AlertsNotifications.Application.Interfaces;
using AlertsNotifications.Domain.Interfaces;

namespace AlertsNotifications.Functions.Workers;

public class CircularExpiryWorker : BackgroundService
{
    private readonly ILogger<CircularExpiryWorker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public CircularExpiryWorker(ILogger<CircularExpiryWorker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CircularExpiryWorker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<ICircularRepository>();
                var messagePublisher = scope.ServiceProvider.GetRequiredService<IMessagePublisher>();

                // Get active circulars that have passed their remove date
                var activeCirculars = await repository.GetByStatusAsync('A', stoppingToken);

                foreach (var circular in activeCirculars)
                {
                    if (circular.CircularRemoveDate.HasValue && circular.CircularRemoveDate.Value <= DateTime.UtcNow)
                    {
                        circular.CircularStatus = 'C';
                        circular.ModifiedOn = DateTime.UtcNow;
                        await repository.UpdateAsync(circular, stoppingToken);

                        await messagePublisher.PublishAsync(
                            "alerts-notifications-exchange",
                            "circular.approval.expired",
                            new
                            {
                                circular.CircularId,
                                circular.CircularSubject,
                                circular.CircularRemoveDate,
                                ProcessedAt = DateTime.UtcNow
                            },
                            stoppingToken);

                        _logger.LogInformation("Circular {CircularId} expired and marked as cancelled.", circular.CircularId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing circular expiry");
            }

            // Run every 6 hours
            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
        }
    }
}
