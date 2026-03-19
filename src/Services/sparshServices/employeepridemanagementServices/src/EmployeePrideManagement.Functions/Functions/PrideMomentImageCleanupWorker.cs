using EmployeePrideManagement.Domain.Interfaces;
using EmployeePrideManagement.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EmployeePrideManagement.Functions.Functions;

public class PrideMomentImageCleanupWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PrideMomentImageCleanupWorker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(12);

    public PrideMomentImageCleanupWorker(IServiceProvider serviceProvider, ILogger<PrideMomentImageCleanupWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PrideMomentImageCleanupWorker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var blobService = scope.ServiceProvider.GetRequiredService<IBlobStorageService>();
                var dbContext = scope.ServiceProvider.GetRequiredService<PrideManagementDbContext>();

                _logger.LogInformation("Running orphaned image cleanup check...");

                // Get all image paths referenced in the database
                var referencedImages = await dbContext.MomentPrides
                    .Select(m => m.Image.Value)
                    .ToListAsync(stoppingToken);

                _logger.LogInformation("Found {Count} referenced images in database.", referencedImages.Count);
                // Orphaned blob cleanup logic can be added here
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during image cleanup.");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}
