using AttendanceService.Application.Commands.AttendanceBatch;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AttendanceService.Functions;

/// <summary>
/// Background worker: Runs monthly attendance batch processing.
/// When deployed to Azure Functions, replace BackgroundService with TimerTrigger-decorated function.
/// Schedule: Runs on the 1st of every month at 01:00 UTC.
/// </summary>
public class AttendanceBatchFunction(IMediator mediator, ILogger<AttendanceBatchFunction> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("AttendanceBatchFunction background worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            // Run on the 1st of every month
            var nextRun = new DateTime(now.Year, now.Month, 1, 1, 0, 0, DateTimeKind.Utc).AddMonths(1);
            var delay = nextRun - now;

            logger.LogInformation("Next attendance batch run scheduled at {NextRun}", nextRun);
            await Task.Delay(delay, stoppingToken);

            if (!stoppingToken.IsCancellationRequested)
                await ProcessBatchAsync(stoppingToken);
        }
    }

    private async Task ProcessBatchAsync(CancellationToken ct)
    {
        try
        {
            logger.LogInformation("Running monthly attendance batch at {Time}", DateTime.UtcNow);
            var monthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var monthEnd = monthStart.AddMonths(1).AddTicks(-1);
            var cmd = new ProcessMonthlyAttendanceCommand(monthStart, monthEnd, ProcessedBy: 0);
            var result = await mediator.Send(cmd, ct);
            logger.LogInformation("Monthly attendance processed. BatchId={BatchId}", result.BatchId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "AttendanceBatchFunction failed.");
        }
    }
}
