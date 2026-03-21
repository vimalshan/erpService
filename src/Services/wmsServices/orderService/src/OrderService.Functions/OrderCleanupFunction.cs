using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Repositories;

namespace OrderService.Functions;

public class OrderCleanupFunction
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrderCleanupFunction> _logger;

    public OrderCleanupFunction(IOrderRepository orderRepository, ILogger<OrderCleanupFunction> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    [Function("OrderCleanup")]
    public async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Order cleanup function triggered at {Time}", DateTime.UtcNow);

        // Example: clean up old cancelled orders or perform archival
        var orders = await _orderRepository.GetAllAsync(cancellationToken);
        var staleOrders = orders.Where(o =>
            o.Status == Domain.Enums.OrderStatus.Cancelled &&
            o.ModifiedDate < DateTime.UtcNow.AddDays(-90)).ToList();

        foreach (var order in staleOrders)
        {
            await _orderRepository.DeleteAsync(order.OrderId, cancellationToken);
            _logger.LogInformation("Archived cancelled order {OrderId}", order.OrderId);
        }

        _logger.LogInformation("Order cleanup completed. Archived {Count} orders.", staleOrders.Count);
    }
}
