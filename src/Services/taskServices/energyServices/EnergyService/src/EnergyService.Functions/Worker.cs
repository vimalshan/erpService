using EnergyService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EnergyService.Functions;

/// <summary>
/// Background worker that periodically aggregates energy readings
/// and generates daily summary reports.
/// </summary>
public class EnergyAggregationWorker(
    ILogger<EnergyAggregationWorker> logger,
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Energy Aggregation Worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                logger.LogInformation("Running energy aggregation at {Time}", DateTimeOffset.UtcNow);

                // Aggregate all processes readings
                var processes = await uow.Processes.GetAllAsync(stoppingToken);
                foreach (var process in processes)
                {
                    if (process.EcCloseFlag == "Y") continue;

                    var readings = await uow.Readings.GetByProcessIdAsync(process.EcProcessId, stoppingToken);
                    var todayReadings = readings.Where(r => r.EbDate.Date == DateTime.UtcNow.Date).ToList();

                    if (todayReadings.Count != 0)
                    {
                        var totalUsage = todayReadings.Sum(r => r.EbActualUsage ?? 0);
                        logger.LogInformation(
                            "Process {ProcessId} ({Desc}): {Count} readings today, total usage: {Usage} {Unit}",
                            process.EcProcessId, process.EcProcessDesc, todayReadings.Count, totalUsage, process.EcUnitCode);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during energy aggregation");
            }

            // Run every hour
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
