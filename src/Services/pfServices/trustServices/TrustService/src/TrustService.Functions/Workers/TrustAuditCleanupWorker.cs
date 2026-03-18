using Microsoft.EntityFrameworkCore;
using TrustService.Infrastructure.Persistence;

namespace TrustService.Functions.Workers;

public class TrustAuditCleanupWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TrustAuditCleanupWorker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(24);

    public TrustAuditCleanupWorker(IServiceProvider serviceProvider, ILogger<TrustAuditCleanupWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("TrustAuditCleanupWorker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<TrustDbContext>();

                // Clean up audit logs older than 90 days
                var cutoffDate = DateTime.UtcNow.AddDays(-90);
                var oldLogs = await context.TrustAuditLogs
                    .Where(a => a.AuditTimestamp < cutoffDate)
                    .ToListAsync(stoppingToken);

                if (oldLogs.Count > 0)
                {
                    context.TrustAuditLogs.RemoveRange(oldLogs);
                    await context.SaveChangesAsync(stoppingToken);
                    _logger.LogInformation("Cleaned up {Count} old audit log entries.", oldLogs.Count);
                }
                else
                {
                    _logger.LogInformation("No old audit logs to clean up.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during audit log cleanup.");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}
