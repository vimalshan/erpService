using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SparshTransactional.Domain.Interfaces;

namespace SparshTransactional.Functions;

public class ApplicationCleanupFunction(
    IScholarshipApplicationRepository applicationRepository,
    ILogger<ApplicationCleanupFunction> logger)
{
    [Function("CleanupRejectedApplications")]
    public async Task Run([TimerTrigger("0 0 2 * * 0")] TimerInfo timer) // Every Sunday at 2 AM
    {
        logger.LogInformation("ApplicationCleanup started at {Time}", DateTime.UtcNow);

        var rejected = await applicationRepository.GetByStatusAsync("R");
        var cutoffDate = DateTime.UtcNow.AddDays(-90);
        var staleApplications = rejected.Where(a => a.UpdatedOn.HasValue && a.UpdatedOn.Value < cutoffDate).ToList();

        logger.LogInformation("Found {Count} stale rejected applications older than 90 days", staleApplications.Count);

        foreach (var app in staleApplications)
        {
            try
            {
                logger.LogInformation("Archiving rejected application {AppId} for student {StudentId}",
                    app.ApplicationId, app.StudentId);
                // Mark as archived rather than deleting
                // In production, this could move to an archive table
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error archiving application {AppId}", app.ApplicationId);
            }
        }

        logger.LogInformation("ApplicationCleanup completed at {Time}", DateTime.UtcNow);
    }
}
