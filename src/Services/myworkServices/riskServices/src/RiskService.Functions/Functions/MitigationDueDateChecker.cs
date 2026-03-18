using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using RiskService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace RiskService.Functions.Functions;

/// <summary>
/// Background task that checks for overdue risk mitigations and generates alerts.
/// Runs every hour.
/// </summary>
public class MitigationDueDateChecker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MitigationDueDateChecker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(1);

    public MitigationDueDateChecker(IServiceProvider serviceProvider, ILogger<MitigationDueDateChecker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("MitigationDueDateChecker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckOverdueMitigationsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking overdue mitigations");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task CheckOverdueMitigationsAsync(CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<RiskDbContext>();

        var overdueMitigations = await context.Mitigations
            .Where(m => m.Status == 'L' && m.DueDate < DateTime.UtcNow)
            .ToListAsync(ct);

        if (overdueMitigations.Any())
        {
            _logger.LogWarning("Found {Count} overdue mitigations", overdueMitigations.Count);
            foreach (var m in overdueMitigations)
            {
                _logger.LogWarning("Overdue mitigation {MitigationId} for Risk {RiskId}, due: {DueDate}", m.Id, m.RiskId, m.DueDate);
            }
        }
        else
        {
            _logger.LogInformation("No overdue mitigations found");
        }
    }
}
