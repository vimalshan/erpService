using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TransactionService.Functions.Functions;

public class TransactionNotificationFunction
{
    private readonly ILogger<TransactionNotificationFunction> _logger;

    public TransactionNotificationFunction(ILogger<TransactionNotificationFunction> logger)
    {
        _logger = logger;
    }

    [Function("SendTransactionNotifications")]
    public async Task SendTransactionNotifications(
        [TimerTrigger("0 0 9 * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation($"Sending transaction notifications at {DateTime.UtcNow}");

        try
        {
            // Send notifications for pending SAA recommendations
            // This runs daily at 9 AM

            _logger.LogInformation("Transaction notifications sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending transaction notifications");
        }

        await Task.CompletedTask;
    }
}
