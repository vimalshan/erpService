using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TimeSheetService.Application.Queries.GetAllTimesheets;

namespace TimeSheetService.Functions.Functions;

/// <summary>Periodic background function: generates timesheet reports daily</summary>
public class TimesheetReportFunction : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TimesheetReportFunction> _logger;
    private static readonly TimeSpan ReportInterval = TimeSpan.FromHours(24);

    public TimesheetReportFunction(IServiceProvider serviceProvider, ILogger<TimesheetReportFunction> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("TimesheetReportFunction started.");

        // Wait until next midnight for first run
        var now = DateTime.UtcNow;
        var nextMidnight = now.Date.AddDays(1);
        var initialDelay = nextMidnight - now;

        await Task.Delay(initialDelay, stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await GenerateReportAsync(stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error during timesheet report generation.");
            }

            await Task.Delay(ReportInterval, stoppingToken);
        }
    }

    private async Task GenerateReportAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var timesheets = await mediator.Send(new GetAllTimesheetsQuery(), cancellationToken);

        var reportDate = DateTime.UtcNow.Date.AddDays(-1);
        var dailyEntries = timesheets.Where(t => t.TimeDate.Date == reportDate).ToList();

        _logger.LogInformation("TimesheetReportFunction: Daily report for {Date} — {Count} entries",
            reportDate.ToShortDateString(), dailyEntries.Count);
    }
}
