using MediatR;
using InventoryService.Application.Queries.GetInventoryByWarehouse;

namespace InventoryService.Functions.Workers;

public class InventorySnapshotWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<InventorySnapshotWorker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(1);

    public InventorySnapshotWorker(IServiceProvider serviceProvider, ILogger<InventorySnapshotWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Inventory Snapshot Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                // Snapshot for warehouses 1-5 (configurable in production)
                for (int warehouseId = 1; warehouseId <= 5; warehouseId++)
                {
                    var inventory = await mediator.Send(
                        new GetInventoryByWarehouseQuery(warehouseId), stoppingToken);

                    var count = inventory.Count();
                    if (count > 0)
                    {
                        _logger.LogInformation(
                            "Inventory snapshot for Warehouse {WarehouseId}: {Count} stock levels recorded.",
                            warehouseId, count);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Inventory Snapshot Worker.");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}
