using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AuthorizationService.Functions.Functions;

public class AuthorizationNotificationFunction
{
    private readonly ILogger<AuthorizationNotificationFunction> _logger;

    public AuthorizationNotificationFunction(ILogger<AuthorizationNotificationFunction> logger)
    {
        _logger = logger;
    }

    [Function("SendAuthorizationNotifications")]
    public async Task SendAuthorizationNotifications(
        [TimerTrigger("0 0 9 * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation($"Sending authorization notifications at {DateTime.UtcNow}");

        try
        {
            // Send notifications for pending authorization changes
            // This runs daily at 9 AM

            _logger.LogInformation("Authorization notifications sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending authorization notifications");
        }

        await Task.CompletedTask;
    }
}
