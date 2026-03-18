using Microsoft.EntityFrameworkCore;
using TrustService.Infrastructure.Persistence;

namespace TrustService.Functions.Workers;

public class TrustExpirationCheckerWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TrustExpirationCheckerWorker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(6);

    public TrustExpirationCheckerWorker(IServiceProvider serviceProvider, ILogger<TrustExpirationCheckerWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("TrustExpirationCheckerWorker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<TrustDbContext>();

                // Find trusts with closure dates in the past that are still active
                var expiredTrusts = await context.TrustMasters
                    .Where(t => t.TrustStatus == "A" &&
                                t.TrustClosureDate != null &&
                                t.TrustClosureDate <= DateTime.UtcNow)
                    .ToListAsync(stoppingToken);

                foreach (var trust in expiredTrusts)
                {
                    trust.Close(trust.TrustClosureDate!.Value);
                    _logger.LogWarning("Trust {TrustCode} has expired. Marked as closed.", trust.TrustCode);
                }

                if (expiredTrusts.Count > 0)
                {
                    // Clear domain events since this is a background cleanup
                    foreach (var trust in expiredTrusts)
                        trust.ClearDomainEvents();

                    await context.SaveChangesAsync(stoppingToken);
                    _logger.LogInformation("Processed {Count} expired trusts.", expiredTrusts.Count);
                }

                // Check for expiring approvers and roles
                var today = DateTime.UtcNow;
                var expiringApprovers = await context.TrustApprovers
                    .Where(a => a.ApproverStatus == "A" &&
                                a.ClsDate != null &&
                                a.ClsDate <= today)
                    .ToListAsync(stoppingToken);

                foreach (var approver in expiringApprovers)
                {
                    approver.Deactivate(approver.ClsDate!.Value);
                    _logger.LogInformation("Approver {ApproverId} deactivated due to expiration.", approver.ApproverId);
                }

                if (expiringApprovers.Count > 0)
                {
                    foreach (var a in expiringApprovers)
                        a.ClearDomainEvents();
                    await context.SaveChangesAsync(stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during trust expiration check.");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}
