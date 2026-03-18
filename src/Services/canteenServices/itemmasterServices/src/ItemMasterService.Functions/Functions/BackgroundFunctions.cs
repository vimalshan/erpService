using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Timer;
using Microsoft.Extensions.Logging;
using ItemMasterService.Application.CQRS.Commands;

namespace ItemMasterService.Functions.Functions;

/// <summary>
/// Timer-triggered Azure Function: runs daily to close expired item prices.
/// </summary>
public class PriceExpiryFunction
{
    private readonly IMediator _mediator;
    private readonly ILogger<PriceExpiryFunction> _logger;

    public PriceExpiryFunction(IMediator mediator, ILogger<PriceExpiryFunction> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>Runs every day at midnight UTC to close expired prices.</summary>
    [Function(nameof(PriceExpiryFunction))]
    public async Task RunAsync([TimerTrigger("0 0 0 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("[PriceExpiryFunction] Triggered at: {Time}", DateTime.UtcNow);

        // Placeholder: query for items with ClosureDate < today and process them
        // In production, inject a repository and query items needing closure
        _logger.LogInformation("[PriceExpiryFunction] Price expiry check complete.");
    }
}

/// <summary>
/// Timer-triggered Azure Function: syncs item master data from external source.
/// </summary>
public class ItemSyncFunction
{
    private readonly ILogger<ItemSyncFunction> _logger;

    public ItemSyncFunction(ILogger<ItemSyncFunction> logger) => _logger = logger;

    /// <summary>Runs every hour to sync item data.</summary>
    [Function(nameof(ItemSyncFunction))]
    public async Task RunAsync([TimerTrigger("0 0 * * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("[ItemSyncFunction] Hourly sync triggered at: {Time}", DateTime.UtcNow);
        // Inject external data service and synchronize items
        await Task.CompletedTask;
    }
}
