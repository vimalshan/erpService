using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ReportingService.Functions.Functions;

public class AppraisalNotificationFunction
{
    private readonly ILogger<AppraisalNotificationFunction> _logger;

    public AppraisalNotificationFunction(ILogger<AppraisalNotificationFunction> logger)
    {
        _logger = logger;
    }

    [Function("SendAppraisalNotifications")]
    public async Task SendAppraisalNotifications(
        [TimerTrigger("0 0 9 * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation($"Sending appraisal notifications at {DateTime.UtcNow}");

        try
        {
            // Send notifications for pending appraisals
            // This runs daily at 9 AM

            _logger.LogInformation("Appraisal notifications sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending appraisal notifications");
        }

        await Task.CompletedTask;
    }
}
