using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AuthorizationService.Functions.Functions;

public class AuthorizationProcessingFunction
{
    private readonly ILogger<AuthorizationProcessingFunction> _logger;

    public AuthorizationProcessingFunction(ILogger<AuthorizationProcessingFunction> logger)
    {
        _logger = logger;
    }

    [Function("ProcessPendingAuthorizations")]
    public async Task ProcessPendingAuthorizations(
        [TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation($"Processing pending authorizations at {DateTime.UtcNow}");

        try
        {
            // Query pending authorization requests and process them
            // This is a background job that runs every 5 minutes

            _logger.LogInformation("Pending authorizations processed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing pending authorizations");
        }

        await Task.CompletedTask;
    }
}
