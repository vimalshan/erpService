namespace LetTransactionService.API.BackgroundServices;

public class LetCleanupService : BackgroundService
{
    private readonly ILogger<LetCleanupService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(24);

    public LetCleanupService(ILogger<LetCleanupService> logger)
        => _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("LET Cleanup Service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RunCleanupAsync(stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error during LET cleanup task.");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    private Task RunCleanupAsync(CancellationToken ct)
    {
        _logger.LogInformation("Running LET background cleanup at {Time}", DateTimeOffset.UtcNow);
        return Task.CompletedTask;
    }
}
