using MamAllocationService.Application.Interfaces;
using MamAllocationService.Application.Queries;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace MamAllocationService.Functions;

public class AllocationReportFunction(IMediator mediator, ILogger<AllocationReportFunction> logger)
{
    [Function("GenerateDailyAllocationReport")]
    public async Task RunDailyReport([TimerTrigger("0 0 6 * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        logger.LogInformation("Daily Allocation Report generation started at {Time}", DateTime.UtcNow);

        var allocations = await mediator.Send(new GetAllAllocationsQuery(), ct);
        var count = allocations.Count();

        logger.LogInformation("Generated report for {Count} allocation records", count);
    }
}

public class BlobCleanupFunction(IBlobStorageService blobService, ILogger<BlobCleanupFunction> logger)
{
    [Function("CleanupOrphanedBlobs")]
    public async Task RunCleanup([TimerTrigger("0 0 2 * * 0")] TimerInfo timerInfo, CancellationToken ct)
    {
        logger.LogInformation("Orphaned blob cleanup started at {Time}", DateTime.UtcNow);
        // Placeholder: In production, check database for orphaned blob references and clean up
        logger.LogInformation("Blob cleanup completed");
        await Task.CompletedTask;
    }
}

public class AllocationSyncFunction(IMediator mediator, IMessagePublisher publisher, ILogger<AllocationSyncFunction> logger)
{
    [Function("SyncAllocationData")]
    public async Task RunSync([TimerTrigger("0 */30 * * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        logger.LogInformation("Allocation data sync started at {Time}", DateTime.UtcNow);

        var allocations = await mediator.Send(new GetAllAllocationsQuery(), ct);

        foreach (var allocation in allocations)
        {
            await publisher.PublishAsync("allocation.sync", new
            {
                allocation.AllDate,
                allocation.AllRm,
                allocation.AllProd,
                allocation.AllCons,
                allocation.AllSale
            }, ct);
        }

        logger.LogInformation("Allocation data sync completed");
    }
}
