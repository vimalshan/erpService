using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RackingSystem.Application.Features.Bins.Queries;
using RackingSystem.Domain.Interfaces;

namespace RackingSystem.Functions;

/// <summary>Timer-triggered Azure Function that checks bin utilization every 15 minutes
/// and logs warnings for bins exceeding 90% capacity.</summary>
public sealed class BinUtilizationTimerFunction
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<BinUtilizationTimerFunction> _logger;

    public BinUtilizationTimerFunction(IUnitOfWork uow, ILogger<BinUtilizationTimerFunction> logger)
    {
        _uow    = uow;
        _logger = logger;
    }

    [Function("BinUtilizationCheck")]
    public async Task Run([TimerTrigger("0 */15 * * * *")] TimerInfo myTimer, CancellationToken ct)
    {
        _logger.LogInformation("BinUtilizationCheck started at {Time}", DateTime.UtcNow);

        var bins = await _uow.Bins.GetAllAsync(ct);
        var fullBins = 0;

        foreach (var bin in bins.Where(b => b.CapacityQty.HasValue && b.CapacityQty > 0))
        {
            var utilization = await _uow.Bins.GetBinUtilizationAsync(bin.Id, ct);
            if (utilization >= 90m)
            {
                fullBins++;
                _logger.LogWarning("Bin {BinId} ({Code}) is at {Utilization:F1}% capacity — status: {Status}",
                    bin.Id, bin.Code, utilization, bin.Status);
            }
        }

        _logger.LogInformation("BinUtilizationCheck complete. {FullBinCount} bins near/at capacity.", fullBins);
    }
}
