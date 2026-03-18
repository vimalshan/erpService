using BankService.Application.Interfaces;
using BankService.Domain.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BankService.Functions;

public class ChequeReconciliationFunction(
    IUnitOfWork unitOfWork,
    ILogger<ChequeReconciliationFunction> logger)
{
    [Function("ChequeReconciliationTimer")]
    public async Task RunAsync([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo)
    {
        logger.LogInformation("Cheque Reconciliation function started at: {Time}", DateTime.UtcNow);

        var outstandingCheques = await unitOfWork.ChequeDetails.GetByStatusAsync("O");
        logger.LogInformation("Found {Count} outstanding cheques to process", outstandingCheques.Count);

        foreach (var cheque in outstandingCheques)
        {
            if (cheque.ChequeDate.HasValue && cheque.ChequeDate.Value.AddDays(90) < DateTime.UtcNow)
            {
                logger.LogWarning("Cheque {ChequeId} is older than 90 days and still outstanding", cheque.ChequeId);
            }
        }

        logger.LogInformation("Cheque Reconciliation function completed at: {Time}", DateTime.UtcNow);
    }
}
