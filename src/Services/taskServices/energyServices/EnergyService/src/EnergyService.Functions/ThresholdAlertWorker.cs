using EnergyService.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace EnergyService.Functions;

/// <summary>
/// Background worker that monitors energy readings for threshold alerts
/// and publishes notifications via RabbitMQ.
/// </summary>
public class ThresholdAlertWorker(
    ILogger<ThresholdAlertWorker> logger,
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Threshold Alert Worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var uow = scope.ServiceProvider.GetRequiredService<Domain.Interfaces.IUnitOfWork>();
                var publisher = scope.ServiceProvider.GetRequiredService<IRabbitMqPublisher>();

                var processes = await uow.Processes.GetAllAsync(stoppingToken);
                foreach (var process in processes)
                {
                    if (process.EcCloseFlag == "Y") continue;

                    var readings = await uow.Readings.GetByProcessIdAsync(process.EcProcessId, stoppingToken);
                    var latestReading = readings.FirstOrDefault();

                    if (latestReading is { EbTarget: not null, EbActualUsage: not null }
                        && latestReading.EbActualUsage > latestReading.EbTarget)
                    {
                        logger.LogWarning(
                            "ALERT: Process {ProcessId} usage ({Usage}) exceeded target ({Target})",
                            process.EcProcessId, latestReading.EbActualUsage, latestReading.EbTarget);

                        await publisher.PublishAsync("energy-exchange", "alert.threshold-exceeded", new
                        {
                            process.EcProcessId,
                            process.EcProcessDesc,
                            ActualUsage = latestReading.EbActualUsage,
                            Target = latestReading.EbTarget,
                            Timestamp = DateTime.UtcNow
                        }, stoppingToken);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during threshold alert check");
            }

            // Run every 30 minutes
            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
}
