namespace SecurityService.Functions.BackgroundTasks;

public class AuditLogWorker : BackgroundService
{
    private readonly ILogger<AuditLogWorker> _logger;

    public AuditLogWorker(ILogger<AuditLogWorker> logger) => _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("AuditLogWorker running at: {Time}", DateTimeOffset.Now);

            // Process audit log entries, archive old logs, etc.
            // This would typically read from a queue or database and process entries

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
}
