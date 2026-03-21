using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WarehouseStructure.Domain.Interfaces;

namespace WarehouseStructure.Functions.Functions;

public class WarehouseCleanupFunction : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<WarehouseCleanupFunction> _logger;

    public WarehouseCleanupFunction(IServiceProvider serviceProvider, ILogger<WarehouseCleanupFunction> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("WarehouseCleanupFunction started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var warehouseRepository = scope.ServiceProvider.GetRequiredService<IWarehouseRepository>();

                var warehouses = await warehouseRepository.GetAllAsync(stoppingToken);
                var inactiveCount = warehouses.Count(w => !w.IsActive);

                _logger.LogInformation("Cleanup check: found {InactiveCount} inactive warehouses.", inactiveCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during warehouse cleanup.");
            }

            // Run every 6 hours
            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
        }
    }
}
