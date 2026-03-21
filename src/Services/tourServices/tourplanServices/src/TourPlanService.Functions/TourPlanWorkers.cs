using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TourPlanService.Infrastructure.Data;

namespace TourPlanService.Functions;

/// <summary>Background worker that checks for upcoming tour plans and sends reminders</summary>
public sealed class TourPlanReminderWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<TourPlanReminderWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("TourPlan Reminder Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessRemindersAsync(stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogError(ex, "Error processing tour plan reminders.");
            }

            // Run every hour
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private async Task ProcessRemindersAsync(CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TourPlanDbContext>();

        var upcomingDate = DateTime.UtcNow.AddDays(2);
        var upcomingTourPlans = dbContext.TourPlans
            .Where(tp => tp.TpStatus == "APPROVED"
                && tp.TpStartDate <= upcomingDate
                && tp.TpStartDate >= DateTime.UtcNow)
            .ToList();

        foreach (var tp in upcomingTourPlans)
        {
            logger.LogInformation(
                "Reminder: Tour plan {TpId} for employee {EmpId} starts on {StartDate}",
                tp.TpId, tp.TpEmpSysId, tp.TpStartDate);
            // TODO: Send email/SMS notification
        }

        logger.LogInformation("Processed {Count} upcoming tour plan reminders.", upcomingTourPlans.Count);
    }
}

/// <summary>Background worker that marks expired tour plans</summary>
public sealed class TourPlanExpiryWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<TourPlanExpiryWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("TourPlan Expiry Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessExpiredAsync(stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogError(ex, "Error processing expired tour plans.");
            }

            // Run every 6 hours
            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
        }
    }

    private async Task ProcessExpiredAsync(CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TourPlanDbContext>();

        var expiredTourPlans = dbContext.TourPlans
            .Where(tp => tp.TpStatus == "APPROVED"
                && tp.TpEndDate.HasValue
                && tp.TpEndDate < DateTime.UtcNow
                && tp.TpExpStatus == null)
            .ToList();

        foreach (var tp in expiredTourPlans)
        {
            logger.LogInformation(
                "Tour plan {TpId} has ended on {EndDate} - expense submission pending.",
                tp.TpId, tp.TpEndDate);
            // TODO: Trigger expense submission notifications
        }
    }
}
