using BusServices.Infrastructure.Persistence;
using BusServices.Infrastructure.Persistence.Dapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BusServices.Functions.Functions;

/// <summary>
/// Runs daily at midnight to process bus arrival status and flag missing arrivals.
/// </summary>
public sealed class BusArrivalProcessorFunction
{
    private readonly BusDbContext _ctx;
    private readonly ILogger<BusArrivalProcessorFunction> _logger;

    public BusArrivalProcessorFunction(BusDbContext ctx, ILogger<BusArrivalProcessorFunction> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    [Function("BusArrivalProcessor")]
    public async Task Run([TimerTrigger("0 0 0 * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        _logger.LogInformation("BusArrivalProcessor triggered at: {Time}", DateTime.UtcNow);

        var yesterday = DateTime.UtcNow.Date.AddDays(-1);
        var activeBuses = await _ctx.Buses.ToListAsync(ct);
        var arrivals = await _ctx.BusArrivals
            .Where(a => a.ArrivalDate == yesterday)
            .ToListAsync(ct);

        var busesWithArrival = arrivals.Select(a => a.BusId).Distinct().ToHashSet();
        var busesWithoutArrival = activeBuses.Where(b => !busesWithArrival.Contains(b.BusId)).ToList();

        if (busesWithoutArrival.Count > 0)
        {
            _logger.LogWarning(
                "{Count} buses had no arrival record for {Date}: {BusIds}",
                busesWithoutArrival.Count,
                yesterday.ToShortDateString(),
                string.Join(", ", busesWithoutArrival.Select(b => b.RegistrationNumber.Value)));
        }
        else
        {
            _logger.LogInformation("All {Count} buses recorded arrivals for {Date}.",
                activeBuses.Count, yesterday.ToShortDateString());
        }
    }
}
