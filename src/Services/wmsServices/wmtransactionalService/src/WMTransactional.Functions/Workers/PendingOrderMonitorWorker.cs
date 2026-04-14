using MediatR;
using WMTransactional.Application.Queries.GetPurchaseOrders;
using WMTransactional.Application.Queries.GetSalesOrders;

namespace WMTransactional.Functions.Workers;

public class PendingOrderMonitorWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PendingOrderMonitorWorker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(15);

    public PendingOrderMonitorWorker(IServiceProvider serviceProvider, ILogger<PendingOrderMonitorWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Pending Order Monitor Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                // Check for draft purchase orders
                var draftPOs = await mediator.Send(new GetPurchaseOrdersQuery { Status = "DRAFT" }, stoppingToken);
                foreach (var po in draftPOs)
                {
                    var age = DateTime.UtcNow - po.OrderDate;
                    if (age.TotalHours > 24)
                    {
                        _logger.LogWarning(
                            "Purchase Order {PoNumber} has been in DRAFT for {Hours:F1} hours.",
                            po.PoNumber, age.TotalHours);
                    }
                }

                // Check for draft sales orders
                var draftSOs = await mediator.Send(new GetSalesOrdersQuery { Status = "DRAFT" }, stoppingToken);
                foreach (var so in draftSOs)
                {
                    var age = DateTime.UtcNow - so.OrderDate;
                    if (age.TotalHours > 24)
                    {
                        _logger.LogWarning(
                            "Sales Order {SoNumber} has been in DRAFT for {Hours:F1} hours.",
                            so.SoNumber, age.TotalHours);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Pending Order Monitor Worker.");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}
