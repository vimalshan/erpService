using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TimeSheetService.Application.Queries.GetAllTimesheets;

namespace TimeSheetService.Functions.Functions;

/// <summary>Periodic background function: checks for missing timesheets and sends notifications</summary>
public class TimesheetNotificationFunction : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TimesheetNotificationFunction> _logger;
    private static readonly TimeSpan CheckInterval = TimeSpan.FromHours(1);

    public TimesheetNotificationFunction(IServiceProvider serviceProvider, ILogger<TimesheetNotificationFunction> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("TimesheetNotificationFunction started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessNotificationsAsync(stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error during timesheet notification processing.");
            }

            await Task.Delay(CheckInterval, stoppingToken);
        }
    }

    private async Task ProcessNotificationsAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var timesheets = await mediator.Send(new GetAllTimesheetsQuery(), cancellationToken);

        _logger.LogInformation("TimesheetNotificationFunction: processed {Count} timesheet entries at {Time}",
            timesheets.Count(), DateTimeOffset.UtcNow);
    }
}
