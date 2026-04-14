using MediatR;
using WMTransactional.Application.Queries.GetShipments;

namespace WMTransactional.Functions.Workers;

public class OverdueShipmentWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OverdueShipmentWorker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(30);

    public OverdueShipmentWorker(IServiceProvider serviceProvider, ILogger<OverdueShipmentWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Overdue Shipment Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                // Check for open shipments (not yet shipped)
                var openShipments = await mediator.Send(new GetShipmentsQuery(), stoppingToken);
                foreach (var shipment in openShipments)
                {
                    if (shipment.Status == "OPEN")
                    {
                        _logger.LogWarning(
                            "Shipment {ShipmentId} for Sales Order {SoId} is still open.",
                            shipment.ShipmentId, shipment.SoId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Overdue Shipment Worker.");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}
