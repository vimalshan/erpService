using MenuAndSecurityService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MenuAndSecurityService.Functions.Workers;

public class MenuCleanupWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<MenuCleanupWorker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(24);

    public MenuCleanupWorker(IServiceScopeFactory scopeFactory, ILogger<MenuCleanupWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("MenuCleanupWorker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await PerformCleanup(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during menu cleanup");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task PerformCleanup(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MenuSecurityDbContext>();

        // Find orphaned menu access entries (no matching menu)
        var orphanedAccesses = await context.RoleMenuAccesses
            .Where(r => !context.MenuMasters.Any(m => m.MenuId == r.MenuId))
            .ToListAsync(ct);

        if (orphanedAccesses.Count > 0)
        {
            context.RoleMenuAccesses.RemoveRange(orphanedAccesses);
            await context.SaveChangesAsync(ct);
            _logger.LogInformation("Cleaned up {Count} orphaned menu access entries", orphanedAccesses.Count);
        }
        else
        {
            _logger.LogInformation("No orphaned menu access entries found");
        }
    }
}
