using IntegrationService.Application.Interfaces;
using IntegrationService.Domain.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace IntegrationService.Functions;

public class OracleSyncFunction(
    IPurchaseOrderRepository poRepository,
    IVendorRepository vendorRepository,
    IMessagePublisher messagePublisher,
    ILogger<OracleSyncFunction> logger)
{
    [Function("SyncOraclePurchaseOrders")]
    public async Task SyncPurchaseOrders(
        [TimerTrigger("0 */5 * * * *")] TimerInfo timerInfo,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Oracle PO sync function triggered at {Time}", DateTime.UtcNow);

        var purchaseOrders = await poRepository.GetAllAsync(cancellationToken);
        var count = 0;

        foreach (var po in purchaseOrders)
        {
            await messagePublisher.PublishAsync("integration.sync", "po.synced",
                new { po.Id, po.PoNumber, SyncedAt = DateTime.UtcNow }, cancellationToken);
            count++;
        }

        logger.LogInformation("Oracle PO sync completed. {Count} records processed", count);
    }

    [Function("SyncOracleVendors")]
    public async Task SyncVendors(
        [TimerTrigger("0 */10 * * * *")] TimerInfo timerInfo,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Oracle Vendor sync function triggered at {Time}", DateTime.UtcNow);

        var vendors = await vendorRepository.GetAllAsync(cancellationToken);
        var count = 0;

        foreach (var vendor in vendors)
        {
            await messagePublisher.PublishAsync("integration.sync", "vendor.synced",
                new { vendor.Id, vendor.VendorName, SyncedAt = DateTime.UtcNow }, cancellationToken);
            count++;
        }

        logger.LogInformation("Oracle Vendor sync completed. {Count} records processed", count);
    }
}
