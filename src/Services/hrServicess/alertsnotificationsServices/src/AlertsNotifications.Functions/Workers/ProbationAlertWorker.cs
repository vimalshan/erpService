using AlertsNotifications.Application.Interfaces;
using AlertsNotifications.Domain.Interfaces;

namespace AlertsNotifications.Functions.Workers;

public class ProbationAlertWorker : BackgroundService
{
    private readonly ILogger<ProbationAlertWorker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ProbationAlertWorker(ILogger<ProbationAlertWorker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ProbationAlertWorker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IProbationConfirmationAlertRepository>();
                var messagePublisher = scope.ServiceProvider.GetRequiredService<IMessagePublisher>();

                var pendingAlerts = await repository.GetPendingAlertsAsync(stoppingToken);

                foreach (var alert in pendingAlerts)
                {
                    if (alert.ProbationDate <= DateTime.UtcNow)
                    {
                        await messagePublisher.PublishAsync(
                            "alerts-notifications-exchange",
                            "alert.notification.probation",
                            new
                            {
                                alert.ProbationId,
                                alert.ProbationEmpSysId,
                                alert.ProbationGrade,
                                alert.ProbationDate,
                                ProcessedAt = DateTime.UtcNow
                            },
                            stoppingToken);

                        alert.AlertSentOn = DateTime.UtcNow;
                        await repository.UpdateAsync(alert, stoppingToken);

                        _logger.LogInformation("Sent probation alert for Employee {EmpId}", alert.ProbationEmpSysId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing probation alerts");
            }

            // Run every hour
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
