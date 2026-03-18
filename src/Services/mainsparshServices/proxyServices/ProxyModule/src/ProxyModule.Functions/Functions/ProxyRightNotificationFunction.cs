using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProxyModule.Infrastructure.Persistence;

namespace ProxyModule.Functions.Functions;

public class ProxyRightNotificationFunction
{
    private readonly ProxyModuleDbContext _context;
    private readonly ILogger<ProxyRightNotificationFunction> _logger;

    public ProxyRightNotificationFunction(ProxyModuleDbContext context, ILogger<ProxyRightNotificationFunction> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Function("ProxyRightExpiryNotification")]
    public async Task Run([TimerTrigger("0 0 8 * * *")] TimerInfo timerInfo) // Runs daily at 8 AM
    {
        _logger.LogInformation("ProxyRightExpiryNotification function started at {Time}", DateTime.UtcNow);

        var warningDate = DateTime.UtcNow.AddDays(7);
        var expiringRights = await _context.ProxyRights
            .Where(p => p.ProxyStatus == "A" &&
                        p.ProxyEndDate.HasValue &&
                        p.ProxyEndDate.Value >= DateTime.UtcNow &&
                        p.ProxyEndDate.Value <= warningDate)
            .ToListAsync();

        foreach (var right in expiringRights)
        {
            _logger.LogInformation(
                "Proxy right {ProxyId} for user {ProxyUserId} -> {DelegatedUserId} expires on {EndDate}",
                right.ProxyId, right.ProxyUserId, right.DelegatedUserId, right.ProxyEndDate);

            // TODO: Send notification via email/message queue
        }

        _logger.LogInformation("ProxyRightExpiryNotification found {Count} expiring proxy rights", expiringRights.Count);
    }
}
