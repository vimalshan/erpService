using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WarehouseStructure.Application.Interfaces;
using WarehouseStructure.Domain.Interfaces;

namespace WarehouseStructure.Functions.Functions;

public class WarehouseDataSyncFunction : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<WarehouseDataSyncFunction> _logger;

    public WarehouseDataSyncFunction(IServiceProvider serviceProvider, ILogger<WarehouseDataSyncFunction> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("WarehouseDataSyncFunction started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var warehouseRepository = scope.ServiceProvider.GetRequiredService<IWarehouseRepository>();
                var messagePublisher = scope.ServiceProvider.GetRequiredService<IMessagePublisher>();

                var warehouses = await warehouseRepository.GetAllAsync(stoppingToken);
                var activeWarehouses = warehouses.Where(w => w.IsActive).ToList();

                _logger.LogInformation("Data sync: publishing {Count} active warehouses.", activeWarehouses.Count);

                await messagePublisher.PublishAsync(
                    "warehouse-events",
                    "warehouse.sync",
                    new { Warehouses = activeWarehouses.Select(w => new { w.Code, w.Name }).ToList(), Timestamp = DateTime.UtcNow },
                    stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during warehouse data sync.");
            }

            // Run every hour
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
