using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SwipeTransactionService.Infrastructure.Messaging;
using SwipeTransactionService.Infrastructure.Persistence;
using SwipeTransactionService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace SwipeTransactionService.Functions;

/// <summary>
/// Timer-triggered function that processes pending swipe uploads every 5 minutes.
/// </summary>
public sealed class ProcessPendingSwipesFunction
{
    private readonly SwipeTransactionDbContext _dbContext;
    private readonly DomainEventDispatcher _eventDispatcher;
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<ProcessPendingSwipesFunction> _logger;

    public ProcessPendingSwipesFunction(
        SwipeTransactionDbContext dbContext,
        DomainEventDispatcher eventDispatcher,
        IMessagePublisher publisher,
        ILogger<ProcessPendingSwipesFunction> logger)
    {
        _dbContext = dbContext;
        _eventDispatcher = eventDispatcher;
        _publisher = publisher;
        _logger = logger;
    }

    [Function(nameof(ProcessPendingSwipesFunction))]
    public async Task Run(
        [TimerTrigger("0 */5 * * * *")] TimerInfo timer,
        CancellationToken ct)
    {
        _logger.LogInformation("Processing pending swipes at {Time}", DateTime.UtcNow);

        var pending = await _dbContext.SwipeCardUploads
            .Where(x => x.UpdateStatus == 'P')
            .ToListAsync(ct);

        if (!pending.Any())
        {
            _logger.LogInformation("No pending swipes to process.");
            return;
        }

        foreach (var upload in pending)
        {
            try
            {
                upload.MarkAsProcessed();
                await _publisher.PublishAsync(
                    "canteen.exchange",
                    "swipe.processed",
                    new { upload.EmployeeNumber, upload.SwipeTime, upload.ItemCode, upload.ItemQuantity },
                    ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process swipe for employee {Emp}", upload.EmployeeNumber);
                upload.MarkAsFailed();
            }
        }

        await _dbContext.SaveChangesAsync(ct);
        await _eventDispatcher.DispatchAndClearAsync(pending, ct);
        _logger.LogInformation("Processed {Count} swipe records.", pending.Count);
    }
}
