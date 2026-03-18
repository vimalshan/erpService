using CSA.Service.Application.Interfaces;
using CSA.Service.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CSA.Service.Functions;

/// <summary>
/// Background worker that sends survey due date reminders.
/// </summary>
public class SurveyReminderWorker(
    ILogger<SurveyReminderWorker> logger,
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("SurveyReminderWorker running at: {Time}", DateTimeOffset.Now);

            try
            {
                using var scope = scopeFactory.CreateScope();
                var surveyRepo = scope.ServiceProvider.GetRequiredService<ISurveyRepository>();
                var publisher = scope.ServiceProvider.GetRequiredService<IMessagePublisher>();

                var surveys = await surveyRepo.GetAllAsync(stoppingToken);
                var dueSoon = surveys.Where(s => s.DueDate <= DateTime.UtcNow.AddDays(3) && s.DueDate > DateTime.UtcNow);

                foreach (var survey in dueSoon)
                {
                    logger.LogInformation("Sending reminder for Survey {SurveyId}: {Title}, Due: {DueDate}",
                        survey.SurveyId, survey.Title, survey.DueDate);
                    await publisher.PublishAsync("csa.events", "survey.reminder",
                        new { survey.SurveyId, survey.Title, survey.DueDate }, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in SurveyReminderWorker");
            }

            // Run every hour
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}

/// <summary>
/// Background worker that cleans up orphaned temporary evidence files.
/// </summary>
public class EvidenceCleanupWorker(
    ILogger<EvidenceCleanupWorker> logger,
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("EvidenceCleanupWorker running at: {Time}", DateTimeOffset.Now);

            try
            {
                using var scope = scopeFactory.CreateScope();
                var dapperService = scope.ServiceProvider.GetRequiredService<IDapperQueryService>();

                // Find orphaned temp evidence older than 24 hours
                var orphanedCount = await dapperService.ExecuteAsync(
                    "DELETE FROM CSA_EVIDENCE WHERE CONTROLEV_NAME IS NULL AND CONTROLEV_TEMPNAME IS NOT NULL",
                    null, stoppingToken);

                if (orphanedCount > 0)
                    logger.LogInformation("Cleaned up {Count} orphaned evidence records", orphanedCount);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in EvidenceCleanupWorker");
            }

            // Run every 6 hours
            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
        }
    }
}
