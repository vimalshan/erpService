using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Document.Functions.Functions;

/// <summary>
/// Background service that cleans up old letter log history records older than 1 year.
/// Runs daily at 2:00 AM UTC (equivalent of the former Azure Functions timer trigger).
/// </summary>
public class LetterCleanupWorker : BackgroundService
{
    private readonly ILogger<LetterCleanupWorker> _logger;

    public LetterCleanupWorker(ILogger<LetterCleanupWorker> logger)
        => _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            var nextRun = now.Date.AddDays(now.Hour >= 2 ? 1 : 0).AddHours(2);
            var delay = nextRun - now;

            _logger.LogInformation("LetterCleanupWorker: next run at {NextRun}", nextRun);
            await Task.Delay(delay, stoppingToken);

            if (stoppingToken.IsCancellationRequested) break;

            _logger.LogInformation("LetterCleanup started at {Time}", DateTime.UtcNow);

            // TODO: inject IApplicationDbContext via IServiceScopeFactory and delete records older than 1 year
            // using var scope = _scopeFactory.CreateScope();
            // var ctx = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            // var cutoff = DateTime.UtcNow.AddYears(-1);
            // ctx.LetterLogHistories.RemoveRange(ctx.LetterLogHistories.Where(l => l.OpenedOn < cutoff));
            // await ctx.SaveChangesAsync(stoppingToken);

            _logger.LogInformation("LetterCleanup completed at {Time}", DateTime.UtcNow);
        }
    }
}
