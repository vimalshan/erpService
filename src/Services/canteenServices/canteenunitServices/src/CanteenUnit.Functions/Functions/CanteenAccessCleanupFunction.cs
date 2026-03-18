using CanteenUnit.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CanteenUnit.Functions.Functions;

/// <summary>
/// Runs weekly to close expired canteen unit accesses.
/// </summary>
public class CanteenAccessCleanupFunction
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CanteenAccessCleanupFunction> _logger;

    public CanteenAccessCleanupFunction(ApplicationDbContext context, ILogger<CanteenAccessCleanupFunction> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task RunAsync(CancellationToken ct)
    {
        _logger.LogInformation("CanteenAccessCleanupFunction executing at {Time}", DateTime.UtcNow);

        // Find all open accesses on units that no longer reference valid HRMS IDs
        var openAccesses = await _context.CanteenUnitAccesses
            .Where(a => a.UnClsDat == null && a.UnEntOn < DateTime.UtcNow.AddYears(-1))
            .ToListAsync(ct);

        foreach (var access in openAccesses)
        {
            access.Revoke();
            _logger.LogInformation("Revoked stale access {Acc} for user {User}", access.UnUntAcc, access.UnUsrId);
        }

        await _context.SaveChangesAsync(ct);
        _logger.LogInformation("Cleanup complete. Revoked {Count} accesses.", openAccesses.Count);
    }
}
