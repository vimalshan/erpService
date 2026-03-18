using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProxyModule.Infrastructure.Persistence;

namespace ProxyModule.Functions.Functions;

public class ProxyRightCleanupFunction
{
    private readonly ProxyModuleDbContext _context;
    private readonly ILogger<ProxyRightCleanupFunction> _logger;

    public ProxyRightCleanupFunction(ProxyModuleDbContext context, ILogger<ProxyRightCleanupFunction> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Function("ProxyRightCleanup")]
    public async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo) // Runs daily at 2 AM
    {
        _logger.LogInformation("ProxyRightCleanup function started at {Time}", DateTime.UtcNow);

        var now = DateTime.UtcNow;
        var expiredRights = await _context.ProxyRights
            .Where(p => p.ProxyStatus == "A" && p.ProxyEndDate.HasValue && p.ProxyEndDate.Value < now)
            .ToListAsync();

        foreach (var right in expiredRights)
        {
            right.Deactivate(0); // System-initiated deactivation
        }

        var count = await _context.SaveChangesAsync();
        _logger.LogInformation("ProxyRightCleanup deactivated {Count} expired proxy rights", count);
    }
}
