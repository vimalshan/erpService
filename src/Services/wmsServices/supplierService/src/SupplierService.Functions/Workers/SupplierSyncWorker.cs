using MediatR;
using SupplierService.Application.Features.Suppliers.Queries;

namespace SupplierService.Functions.Workers;

public class SupplierSyncWorker : BackgroundService
{
    private readonly ILogger<SupplierSyncWorker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public SupplierSyncWorker(ILogger<SupplierSyncWorker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Supplier sync worker running at: {Time}", DateTimeOffset.Now);

            try
            {
                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var suppliers = await mediator.Send(new GetAllSuppliersQuery(), stoppingToken);
                _logger.LogInformation("Synced {Count} suppliers", suppliers.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during supplier sync");
            }

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
}
