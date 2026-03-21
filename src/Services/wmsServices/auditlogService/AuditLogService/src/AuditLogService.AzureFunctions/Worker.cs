using AuditLogService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuditLogService.AzureFunctions;

/// <summary>
/// Background worker that performs periodic audit log archival/cleanup.
/// </summary>
public class AuditLogArchivalWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<AuditLogArchivalWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Audit log archival worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AuditLogDbContext>();

                var cutoffDate = DateTime.UtcNow.AddDays(-90);
                var oldLogs = await context.AuditLogs
                    .Where(l => l.ChangeDate < cutoffDate)
                    .ToListAsync(stoppingToken);

                if (oldLogs.Count > 0)
                {
                    logger.LogInformation("Archiving {Count} audit logs older than {CutoffDate}", oldLogs.Count, cutoffDate);
                    context.AuditLogs.RemoveRange(oldLogs);
                    await context.SaveChangesAsync(stoppingToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during audit log archival.");
            }

            // Run every 24 hours
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}

