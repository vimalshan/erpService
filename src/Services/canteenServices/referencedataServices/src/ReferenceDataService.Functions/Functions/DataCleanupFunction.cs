using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReferenceDataService.Infrastructure.Persistence;

namespace ReferenceDataService.Functions.Functions;

public class DataCleanupFunction : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<DataCleanupFunction> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(24);

    public DataCleanupFunction(IServiceScopeFactory scopeFactory, ILogger<DataCleanupFunction> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("DataCleanupFunction started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Running data cleanup at {Time}", DateTimeOffset.UtcNow);

                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ReferenceDataDbContext>();

                // Perform cleanup tasks here (e.g., remove orphaned records, audit log cleanup)
                _logger.LogInformation("Data cleanup completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during data cleanup");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}
