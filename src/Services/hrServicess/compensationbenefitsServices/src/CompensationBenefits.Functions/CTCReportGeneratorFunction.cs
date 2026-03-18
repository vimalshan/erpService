using CompensationBenefits.Infrastructure.Dapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace CompensationBenefits.Functions;

/// <summary>
/// Timer-triggered function that generates monthly CTC reports and stores them in Azure Blob Storage.
/// Triggers at 1:00 AM UTC on 1st of every month — "0 0 1 1 * *"
/// </summary>
public class CTCReportGeneratorFunction(IDapperRepository dapperRepository, ILogger<CTCReportGeneratorFunction> logger)
{
    [Function("CTCReportGeneratorFunction")]
    public async Task Run([TimerTrigger("0 0 1 1 * *")] TimerInfo timerInfo, CancellationToken cancellationToken)
    {
        logger.LogInformation("CTCReportGeneratorFunction triggered at {Time} (IsPastDue={IsPastDue})",
            DateTime.UtcNow, timerInfo.IsPastDue);

        try
        {
            // Pull a summary of all active employees' CTC breakdowns using Dapper
            var report = await dapperRepository.GetStructureDetailsAsync(0);

            logger.LogInformation("CTC report generated with {Count} records.", report?.Count() ?? 0);

            // In production: serialize and upload report to Azure Blob via IBlobStorageService
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error generating CTC report.");
            throw;
        }
    }
}
