using MediatR;
using StipendService.Application.Features.StipendDisbursement.Commands;
using StipendService.Domain.ValueObjects;

namespace StipendService.Functions;

/// <summary>
/// Background worker that auto-calculates and processes SRF stipend disbursements
/// on the 1st of each month at midnight UTC.
/// </summary>
public class StipendMonthlyWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<StipendMonthlyWorker> _logger;

    public StipendMonthlyWorker(IServiceProvider serviceProvider, ILogger<StipendMonthlyWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("StipendMonthlyWorker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            // Schedule next run: 1st of next month, 00:05 UTC
            var nextRun = new DateTime(now.Year, now.Month, 1, 0, 5, 0, DateTimeKind.Utc).AddMonths(1);
            var delay = nextRun - now;

            if (delay < TimeSpan.Zero) delay = TimeSpan.FromSeconds(30); // failsafe

            _logger.LogInformation("Next stipend calculation scheduled at {NextRun}.", nextRun);

            try
            {
                await Task.Delay(delay, stoppingToken);

                if (stoppingToken.IsCancellationRequested) break;

                var monthYear = MonthYear.FromDate(DateTime.UtcNow).Value;
                _logger.LogInformation("Running stipend calculation for {MonthYear}.", monthYear);

                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var calcResult = await mediator.Send(
                    new CalculateAndDisburseStipendCommand(monthYear, 1 /* system user */),
                    stoppingToken);

                _logger.LogInformation("Stipend calculation complete: {RowsCreated} records created.", calcResult.RowsCreated);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during monthly stipend calculation.");
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
