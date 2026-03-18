using ErrorLoggingService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ErrorLoggingService.API.Functions;

/// <summary>
/// Background hosted service that periodically purges error log records older than the configured retention period.
/// Acts as the Azure Functions-style background task within the API host.
/// </summary>
public sealed class ErrorLogCleanupFunction : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ErrorLogCleanupFunction> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(24);
    private readonly int _retentionDays = 90;

    public ErrorLogCleanupFunction(IServiceScopeFactory scopeFactory, ILogger<ErrorLogCleanupFunction> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ErrorLogCleanupFunction started. Interval: {Interval}, Retention: {Days} days.", _interval, _retentionDays);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await DoCleanupAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during error log cleanup.");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task DoCleanupAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var cutoff = DateTime.UtcNow.AddDays(-_retentionDays);
        var deleted = await db.ErrorLogs
            .Where(e => e.ErrorDate < cutoff)
            .ExecuteDeleteAsync(cancellationToken);

        _logger.LogInformation("Cleanup: Deleted {Count} error log entries older than {Cutoff}.", deleted, cutoff);
    }
}
