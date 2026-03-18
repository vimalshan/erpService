using DeductionService.Application.CQRS.Commands.ProcessMonthlyDeduction;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DeductionService.Functions;

/// <summary>
/// Background worker that triggers monthly canteen deduction processing.
/// Runs on the 1st day of each month at 02:00 UTC (equivalent to Azure Function TimerTrigger "0 0 2 1 * *").
/// Deploy as Azure Container App Job, Azure Function, or a hosted Worker Service.
/// </summary>
public class ProcessMonthlyDeductionWorker(
    IServiceProvider serviceProvider,
    ILogger<ProcessMonthlyDeductionWorker> logger)
    : BackgroundService
{
    private const long ServiceAccountUserId = 1;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("[Worker] ProcessMonthlyDeductionWorker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            var nextRun = GetNextRunTime(now);
            var delay = nextRun - now;

            logger.LogInformation("[Worker] Next monthly deduction run scheduled at {NextRun} (in {Delay:hh\\:mm\\:ss})",
                nextRun, delay);

            try
            {
                await Task.Delay(delay, stoppingToken);
                await RunMonthlyDeductionsAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }

        logger.LogInformation("[Worker] ProcessMonthlyDeductionWorker stopped.");
    }

    private async Task RunMonthlyDeductionsAsync(CancellationToken ct)
    {
        var monthYear = DateTime.UtcNow.AddMonths(-1).ToString("yyyy-MM");
        logger.LogInformation("[Worker] Processing deductions for period {MonthYear}", monthYear);

        try
        {
            using var scope = serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var result = await mediator.Send(
                new ProcessMonthlyDeductionCommand(monthYear, ServiceAccountUserId), ct);

            logger.LogInformation(
                "[Worker] Deduction processing complete. Period={MonthYear}, Count={Count}, Total={Total}",
                result.MonthYear, result.ProcessedCount, result.TotalAmount);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[Worker] Deduction processing failed for {MonthYear}", monthYear);
        }
    }

    /// <summary>Calculates the next 02:00 UTC on the 1st of the next month.</summary>
    private static DateTime GetNextRunTime(DateTime now)
    {
        var candidate = new DateTime(now.Year, now.Month, 1, 2, 0, 0, DateTimeKind.Utc);
        if (candidate <= now) candidate = candidate.AddMonths(1);
        return candidate;
    }
}

