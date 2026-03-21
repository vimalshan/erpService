using MediatR;
using InventoryService.Application.Queries.GetLowStockItems;
using MassTransit;
using InventoryService.Infrastructure.Messaging;

namespace InventoryService.Functions.Workers;

public class LowStockMonitorWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<LowStockMonitorWorker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(15);

    public LowStockMonitorWorker(IServiceProvider serviceProvider, ILogger<LowStockMonitorWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Low Stock Monitor Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

                var lowStockItems = await mediator.Send(new GetLowStockItemsQuery(), stoppingToken);

                foreach (var item in lowStockItems)
                {
                    _logger.LogWarning(
                        "Low stock detected: Product {ProductId}, Warehouse {WarehouseId}, Bin {BinId}, Available: {Qty}",
                        item.ProductId, item.WarehouseId, item.BinId, item.QuantityAvailable);

                    await publishEndpoint.Publish(new LowStockAlertMessage
                    {
                        ProductId = item.ProductId,
                        WarehouseId = item.WarehouseId,
                        BinId = item.BinId,
                        CurrentQuantity = item.QuantityAvailable,
                        ReorderLevel = item.ReorderLevel ?? 0,
                        OccurredOn = DateTime.UtcNow
                    }, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Low Stock Monitor Worker.");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}
