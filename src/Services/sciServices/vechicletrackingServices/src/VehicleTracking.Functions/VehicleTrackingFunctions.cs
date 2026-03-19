using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VehicleTracking.Domain.Interfaces;

namespace VehicleTracking.Functions;

public class VehicleTrackingFunctions(ILogger<VehicleTrackingFunctions> logger, IUnitOfWork unitOfWork)
{
    [Function("CleanupExpiredTransactions")]
    public async Task CleanupExpiredTransactions(
        [TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo) // Runs daily at 2 AM
    {
        logger.LogInformation("CleanupExpiredTransactions function executed at: {Time}", DateTime.UtcNow);

        var transactions = await unitOfWork.VehicleTransactions.GetActiveTransactionsAsync();
        var expiredCount = 0;

        foreach (var transaction in transactions)
        {
            if (transaction.ReportDate.HasValue && transaction.ReportDate.Value.AddDays(30) < DateTime.UtcNow)
            {
                transaction.VehicleStatus = 'E'; // Expired
                await unitOfWork.VehicleTransactions.UpdateAsync(transaction);
                expiredCount++;
            }
        }

        if (expiredCount > 0)
            await unitOfWork.SaveChangesAsync();

        logger.LogInformation("Cleaned up {Count} expired transactions", expiredCount);
    }

    [Function("GenerateDailyReport")]
    public async Task GenerateDailyReport(
        [TimerTrigger("0 0 6 * * *")] TimerInfo timerInfo) // Runs daily at 6 AM
    {
        logger.LogInformation("GenerateDailyReport function executed at: {Time}", DateTime.UtcNow);

        var activeTransactions = await unitOfWork.VehicleTransactions.GetActiveTransactionsAsync();
        var count = activeTransactions.Count();

        logger.LogInformation("Daily Report: {Count} active vehicle transactions", count);
    }

    [Function("ProcessBlobUpload")]
    public void ProcessBlobUpload(
        [BlobTrigger("vehicle-images/{name}")] Stream stream,
        string name)
    {
        logger.LogInformation("Processing uploaded blob: {Name}, Size: {Size} bytes", name, stream.Length);
        // Add image processing logic here (resize, thumbnail generation, etc.)
    }
}
