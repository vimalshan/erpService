using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using RiskService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace RiskService.Functions.Functions;

/// <summary>
/// Background task that checks for upcoming self-assessment due dates
/// and generates reminders. Runs daily.
/// </summary>
public class SelfAssessmentReminderService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SelfAssessmentReminderService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(24);

    public SelfAssessmentReminderService(IServiceProvider serviceProvider, ILogger<SelfAssessmentReminderService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("SelfAssessmentReminderService started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckUpcomingAssessmentsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking upcoming self assessments");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task CheckUpcomingAssessmentsAsync(CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<RiskDbContext>();

        var upcomingDate = DateTime.UtcNow.AddDays(7);
        var upcomingAssessments = await context.SelfAssessments
            .Where(a => (a.Status == 'E' || a.Status == 'P') && a.DueDate <= upcomingDate)
            .ToListAsync(ct);

        if (upcomingAssessments.Any())
        {
            _logger.LogWarning("Found {Count} self assessments due within 7 days", upcomingAssessments.Count);
            foreach (var a in upcomingAssessments)
            {
                _logger.LogWarning("Assessment {AssessmentId} due on {DueDate}", a.Id, a.DueDate);
            }
        }
        else
        {
            _logger.LogInformation("No upcoming self assessments found");
        }
    }
}
