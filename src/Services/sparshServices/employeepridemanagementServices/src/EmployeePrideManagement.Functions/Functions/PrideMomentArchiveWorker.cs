using EmployeePrideManagement.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EmployeePrideManagement.Functions.Functions;

public class PrideMomentArchiveWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PrideMomentArchiveWorker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(24);

    public PrideMomentArchiveWorker(IServiceProvider serviceProvider, ILogger<PrideMomentArchiveWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PrideMomentArchiveWorker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<PrideManagementDbContext>();

                var archiveDate = DateTime.UtcNow.AddYears(-2);
                var oldMoments = await dbContext.MomentPrides
                    .Where(m => m.ModifiedOn < archiveDate)
                    .ToListAsync(stoppingToken);

                if (oldMoments.Any())
                {
                    _logger.LogInformation("Found {Count} pride moments older than 2 years for archival.", oldMoments.Count);
                    // Archive logic: move to archive table, blob storage, etc.
                    // For now, just log
                    foreach (var moment in oldMoments)
                    {
                        _logger.LogInformation("Archiving pride moment {Id}: {Title}", moment.MomentPrideId, moment.Title);
                    }
                }
                else
                {
                    _logger.LogInformation("No pride moments to archive.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during pride moment archival.");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}
