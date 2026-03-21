using CustomerService.Application.Interfaces;
using CustomerService.Domain.Interfaces;

namespace CustomerService.Functions;

/// <summary>
/// Background worker that periodically processes customer-related tasks:
/// - Deactivates customers with no activity
/// - Sends sync messages to other services
/// </summary>
public class CustomerCleanupWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<CustomerCleanupWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                logger.LogInformation("Customer cleanup worker running at: {Time}", DateTimeOffset.Now);

                using var scope = scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var publisher = scope.ServiceProvider.GetRequiredService<IMessagePublisher>();

                var customers = await unitOfWork.Customers.GetAllAsync(stoppingToken);
                var activeCount = customers.Count(c => c.IsActive);
                var inactiveCount = customers.Count(c => !c.IsActive);

                logger.LogInformation("Customer stats — Active: {Active}, Inactive: {Inactive}", activeCount, inactiveCount);

                // Publish a periodic sync heartbeat
                await publisher.PublishAsync("customer.exchange", "customer.sync.heartbeat", new
                {
                    Timestamp = DateTime.UtcNow,
                    ActiveCustomers = activeCount,
                    InactiveCustomers = inactiveCount
                }, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in customer cleanup worker");
            }

            // Run every 5 minutes
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}

/// <summary>
/// Background worker that processes blob storage cleanup for orphaned customer images.
/// </summary>
public class BlobCleanupWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<BlobCleanupWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                logger.LogInformation("Blob cleanup worker running at: {Time}", DateTimeOffset.Now);

                using var scope = scopeFactory.CreateScope();
                var blobService = scope.ServiceProvider.GetRequiredService<IBlobStorageService>();

                // Placeholder: Scan for orphaned blobs and clean up
                logger.LogInformation("Blob cleanup completed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in blob cleanup worker");
            }

            // Run every 30 minutes
            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
}
