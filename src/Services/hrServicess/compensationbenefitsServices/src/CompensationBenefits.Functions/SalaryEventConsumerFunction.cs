using CompensationBenefits.Infrastructure.Messaging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CompensationBenefits.Functions;

/// <summary>
/// Service Bus–triggered function that processes incoming salary-related events
/// from the compensation.salary.events queue.
/// </summary>
public class SalaryEventConsumerFunction(ISalaryEventProcessor processor, ILogger<SalaryEventConsumerFunction> logger)
{
    [Function("SalaryEventConsumerFunction")]
    public async Task Run(
        [ServiceBusTrigger("compensation.salary.events", Connection = "ServiceBusConnection")] string messageBody,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("SalaryEventConsumerFunction received message at {Time}", DateTime.UtcNow);

        try
        {
            await processor.ProcessAsync(messageBody, cancellationToken);
            logger.LogInformation("Salary event processed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing salary event. Body: {Body}", messageBody);
            throw;
        }
    }
}
