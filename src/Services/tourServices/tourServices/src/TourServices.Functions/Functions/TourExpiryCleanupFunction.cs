using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TourServices.Infrastructure.Persistence;

namespace TourServices.Functions.Functions;

/// <summary>
/// Daily timer function that cancels expired tours (end date passed, still active).
/// Schedule: Every day at 2:00 AM UTC  (cron: 0 0 2 * * *)
/// </summary>
public sealed class TourExpiryCleanupFunction
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TourExpiryCleanupFunction> _logger;

    public TourExpiryCleanupFunction(ApplicationDbContext context,
        ILogger<TourExpiryCleanupFunction> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Function(nameof(TourExpiryCleanupFunction))]
    public async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        _logger.LogInformation("Tour expiry cleanup started at {Time}", DateTime.UtcNow);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var expiredTours = await _context.TourPackages
            .Where(t => EF.Property<string>(t, "TOUR_STATUS") == "A" &&
                        t.EndDate < today)
            .ToListAsync(ct);

        foreach (var tour in expiredTours)
        {
            tour.Complete(updatedBy: 0);
            _logger.LogInformation("Marked tour {TourId} ({TourName}) as completed", tour.TourId, tour.TourName);
        }

        if (expiredTours.Count > 0)
            await _context.SaveChangesAsync(ct);

        _logger.LogInformation("Tour expiry cleanup finished. {Count} tours processed.", expiredTours.Count);
    }
}
