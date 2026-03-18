using ContributionService.Domain.Interfaces;
using ContributionService.Domain.Entities;

namespace ContributionService.Functions.BackgroundTasks;

public class ContributionLogCleanupTask(
    IServiceScopeFactory scopeFactory,
    ILogger<ContributionLogCleanupTask> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Contribution Log Cleanup Task started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Run daily at 3 AM
                var now = DateTime.UtcNow;
                if (now.Hour == 3 && now.Minute == 0)
                {
                    logger.LogInformation("Running contribution log cleanup");
                    using var scope = scopeFactory.CreateScope();
                    var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    // Log cleanup activity
                    await uow.ProcessLogs.AddAsync(
                        ContributionProcessLog.Create("CLEANUP", "Log cleanup executed", 0), stoppingToken);
                    await uow.SaveChangesAsync(stoppingToken);

                    logger.LogInformation("Contribution log cleanup completed");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in contribution log cleanup");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
