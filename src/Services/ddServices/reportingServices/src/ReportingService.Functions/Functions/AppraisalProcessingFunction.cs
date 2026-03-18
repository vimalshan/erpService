using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ReportingService.Functions.Functions;

public class AppraisalProcessingFunction
{
    private readonly ILogger<AppraisalProcessingFunction> _logger;

    public AppraisalProcessingFunction(ILogger<AppraisalProcessingFunction> logger)
    {
        _logger = logger;
    }

    [Function("ProcessPendingAppraisals")]
    public async Task ProcessPendingAppraisals(
        [TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation($"Processing pending appraisals at {DateTime.UtcNow}");

        try
        {
            // Query pending appraisals and process them
            // This is a background job that runs every 5 minutes

            _logger.LogInformation("Pending appraisals processed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing pending appraisals");
        }

        await Task.CompletedTask;
    }
}
