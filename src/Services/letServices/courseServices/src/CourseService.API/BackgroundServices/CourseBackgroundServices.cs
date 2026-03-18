using CourseService.Domain.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CourseService.API.BackgroundServices;

/// <summary>
/// Azure Function-style background service for processing course enrollment reminders.
/// In production, deploy as an Azure Function with a Timer Trigger.
/// </summary>
public class CourseReminderService(IServiceProvider serviceProvider, ILogger<CourseReminderService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("CourseReminderService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessCoursesAsync(stoppingToken);
            }
            catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
            {
                logger.LogError(ex, "Error in CourseReminderService.");
            }

            // Run every hour (simulates Azure Function timer trigger: 0 0 * * * *)
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private async Task ProcessCoursesAsync(CancellationToken ct)
    {
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        // In production: query upcoming courses and send email/push notifications
        logger.LogInformation("CourseReminderService: Checking for upcoming courses at {Time}", DateTime.UtcNow);
        await Task.CompletedTask;
    }
}

/// <summary>
/// Azure Function-style background service for processing course completion reports.
/// </summary>
public class CourseReportService(ILogger<CourseReportService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("CourseReportService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Simulate CRON: daily at midnight (0 0 0 * * *)
                var now = DateTime.UtcNow;
                var nextRun = now.Date.AddDays(1);
                var delay = nextRun - now;

                await Task.Delay(delay, stoppingToken);

                logger.LogInformation("CourseReportService: Generating daily course reports at {Time}", DateTime.UtcNow);
                // In production: generate PDF reports and upload to Azure Blob Storage
            }
            catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
            {
                logger.LogError(ex, "Error in CourseReportService.");
            }
        }
    }
}
