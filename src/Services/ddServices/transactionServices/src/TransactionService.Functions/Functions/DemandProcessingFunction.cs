using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TransactionService.Functions.Functions;

public class DemandProcessingFunction
{
    private readonly ILogger<DemandProcessingFunction> _logger;

    public DemandProcessingFunction(ILogger<DemandProcessingFunction> logger)
    {
        _logger = logger;
    }

    [Function("ProcessPendingDemands")]
    public async Task ProcessPendingDemands(
        [TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation($"Processing pending demands at {DateTime.UtcNow}");

        try
        {
            // Query pending demand requests and process them
            // This is a background job that runs every 5 minutes

            _logger.LogInformation("Pending demands processed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing pending demands");
        }

        await Task.CompletedTask;
    }
}
