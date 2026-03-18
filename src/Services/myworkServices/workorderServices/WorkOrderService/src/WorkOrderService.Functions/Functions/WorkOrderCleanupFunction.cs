using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WorkOrderService.Domain.ValueObjects;
using WorkOrderService.Infrastructure.Persistence;

namespace WorkOrderService.Functions.Functions;

public class WorkOrderCleanupFunction : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<WorkOrderCleanupFunction> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(24);

    public WorkOrderCleanupFunction(IServiceProvider serviceProvider, ILogger<WorkOrderCleanupFunction> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("WorkOrderCleanupFunction running at {Time}", DateTimeOffset.UtcNow);
                await ArchiveClosedWorkOrders(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during work order cleanup");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task ArchiveClosedWorkOrders(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WorkOrderDbContext>();

        var closedStatus = WorkOrderStatus.Closed;
        var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

        var workOrdersToArchive = await context.WorkOrders
            .Where(w => w.WorkOrderStatus == closedStatus && w.UpdatedOn < thirtyDaysAgo)
            .ToListAsync(cancellationToken);

        foreach (var workOrder in workOrdersToArchive)
        {
            workOrder.Archive(0); // System archive
            _logger.LogInformation("Archiving work order {WorkOrderId}: {WorkOrderName}",
                workOrder.WorkOrderId, workOrder.WorkOrderName);
        }

        if (workOrdersToArchive.Count > 0)
        {
            await context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Archived {Count} closed work orders", workOrdersToArchive.Count);
        }
    }
}
