using CalendarService.Infrastructure.Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CalendarService.API.Functions;

/// <summary>
/// Background hosted service simulating an Azure Function timer trigger
/// for sending holiday reminders.
/// </summary>
public class HolidayReminderFunction(IServiceScopeFactory scopeFactory, ILogger<HolidayReminderFunction> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RunAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in HolidayReminderFunction");
            }

            // Simulate timer: run every 24 hours
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

    private async Task RunAsync(CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var dapper = scope.ServiceProvider.GetRequiredService<DapperReadService>();

        var upcoming = await dapper.GetUpcomingHolidaysAsync(7);
        foreach (var h in upcoming)
            logger.LogInformation("[HolidayReminder] Upcoming holiday in 7 days: {Description} on {Date}", h.Description, h.HolidayDate);
    }
}

/// <summary>
/// Background hosted service for periodic shift cache warm-up.
/// </summary>
public class ShiftCacheWarmupFunction(IServiceScopeFactory scopeFactory, ILogger<ShiftCacheWarmupFunction> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var dapper = scope.ServiceProvider.GetRequiredService<DapperReadService>();
                var shifts = await dapper.GetShiftSummariesAsync();
                logger.LogInformation("[ShiftCacheWarmup] Loaded {Count} shift summaries", shifts.Count());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in ShiftCacheWarmupFunction");
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
