using CategoryAndVendorService.Application.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CategoryAndVendorService.Functions;

public class CategorySyncFunction
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<CategorySyncFunction> _logger;

    public CategorySyncFunction(IMessagePublisher publisher, ILogger<CategorySyncFunction> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    [Function("SyncCategories")]
    public async Task Run([TimerTrigger("0 0 */12 * * *")] TimerInfo myTimer, CancellationToken ct)
    {
        _logger.LogInformation("Category sync function triggered at {Time}", DateTime.UtcNow);

        await _publisher.PublishAsync("category-sync", new
        {
            Action = "sync",
            Timestamp = DateTime.UtcNow
        }, ct);

        _logger.LogInformation("Category sync message published");
    }
}
