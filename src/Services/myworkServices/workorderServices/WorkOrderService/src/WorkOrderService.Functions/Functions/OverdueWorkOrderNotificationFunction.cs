using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WorkOrderService.Application.Interfaces;
using WorkOrderService.Domain.ValueObjects;
using WorkOrderService.Infrastructure.Persistence;

namespace WorkOrderService.Functions.Functions;

public class OverdueWorkOrderNotificationFunction : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OverdueWorkOrderNotificationFunction> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(6);

    public OverdueWorkOrderNotificationFunction(IServiceProvider serviceProvider, ILogger<OverdueWorkOrderNotificationFunction> logger)
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
                _logger.LogInformation("OverdueNotificationFunction running at {Time}", DateTimeOffset.UtcNow);
                await NotifyOverdueWorkOrders(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during overdue notification check");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }

    private async Task NotifyOverdueWorkOrders(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WorkOrderDbContext>();
        var publisher = scope.ServiceProvider.GetService<IMessagePublisher>();

        var openStatus = WorkOrderStatus.Open;
        var today = DateTime.UtcNow.Date;

        var overdueOrders = await context.WorkOrders
            .Where(w => w.WorkOrderStatus == openStatus && w.DueDate < today)
            .ToListAsync(cancellationToken);

        foreach (var order in overdueOrders)
        {
            _logger.LogWarning("Work order {WorkOrderId} '{WorkOrderName}' is overdue (due: {DueDate})",
                order.WorkOrderId, order.WorkOrderName, order.DueDate);

            if (publisher is not null)
            {
                await publisher.PublishAsync("workorder.notifications.overdue", new
                {
                    order.WorkOrderId,
                    order.WorkOrderName,
                    order.DueDate,
                    order.AssignedTo,
                    DetectedAt = DateTime.UtcNow
                }, cancellationToken);
            }
        }

        if (overdueOrders.Count > 0)
        {
            _logger.LogInformation("Found {Count} overdue work orders", overdueOrders.Count);
        }
    }
}
