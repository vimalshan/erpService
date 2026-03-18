using Microsoft.Extensions.Hosting;

namespace DevelopmentService.API.BackgroundServices;

public class DevelopmentCleanupService : BackgroundService
{
    private readonly ILogger<DevelopmentCleanupService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(24);

    public DevelopmentCleanupService(ILogger<DevelopmentCleanupService> logger)
        => _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Development Cleanup Service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RunCleanupAsync(stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error during development cleanup task.");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    private Task RunCleanupAsync(CancellationToken ct)
    {
        // Placeholder: archive stale pending plans older than 90 days,
        // generate daily development summary reports, etc.
        _logger.LogInformation("Running development background cleanup at {Time}", DateTimeOffset.UtcNow);
        return Task.CompletedTask;
    }
}
