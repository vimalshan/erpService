using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SparshTransactional.Domain.Interfaces;

namespace SparshTransactional.Functions;

public class DisbursementProcessorFunction(
    IScholarshipDisbursementRepository disbursementRepository,
    IScholarshipApplicationRepository applicationRepository,
    ILogger<DisbursementProcessorFunction> logger)
{
    [Function("ProcessPendingDisbursements")]
    public async Task Run([TimerTrigger("0 */30 * * * *")] TimerInfo timer) // Every 30 minutes
    {
        logger.LogInformation("DisbursementProcessor started at {Time}", DateTime.UtcNow);

        var pending = await disbursementRepository.GetByStatusAsync("P");
        logger.LogInformation("Found {Count} pending disbursements", pending.Count);

        foreach (var disbursement in pending)
        {
            try
            {
                // Verify the application is still approved
                var application = await applicationRepository.GetByIdAsync(disbursement.ApplicationId);
                if (application is null || application.ApplicationStatus != "A")
                {
                    logger.LogWarning("Application {AppId} not valid for disbursement {DisbId}",
                        disbursement.ApplicationId, disbursement.DisbursementId);
                    disbursement.Fail();
                    await disbursementRepository.UpdateAsync(disbursement);
                    continue;
                }

                // Simulate payment processing
                var paymentRef = $"PAY-{DateTime.UtcNow:yyyyMMdd}-{disbursement.DisbursementId}";
                disbursement.Complete(paymentRef);
                await disbursementRepository.UpdateAsync(disbursement);

                logger.LogInformation("Disbursement {DisbId} completed with reference {Ref}",
                    disbursement.DisbursementId, paymentRef);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing disbursement {DisbId}", disbursement.DisbursementId);
            }
        }

        logger.LogInformation("DisbursementProcessor completed at {Time}", DateTime.UtcNow);
    }
}
