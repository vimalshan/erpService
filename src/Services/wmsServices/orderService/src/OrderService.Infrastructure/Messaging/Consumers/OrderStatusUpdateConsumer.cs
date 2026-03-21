using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrderService.Application.DTOs;

namespace OrderService.Infrastructure.Messaging.Consumers;

public class OrderStatusUpdateConsumer : RabbitMqConsumerBase<UpdateOrderStatusMessage>
{
    private readonly ILogger<OrderStatusUpdateConsumer> _logger;

    public OrderStatusUpdateConsumer(IConfiguration configuration, ILogger<OrderStatusUpdateConsumer> logger)
        : base(configuration, logger, "order-status-updates")
    {
        _logger = logger;
    }

    protected override Task HandleMessageAsync(UpdateOrderStatusMessage message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received order status update: OrderId={OrderId}, Status={Status}",
            message.OrderId, message.Status);
        // Process the status update — integrate with MediatR or repository as needed
        return Task.CompletedTask;
    }
}

public record UpdateOrderStatusMessage(int OrderId, string Status);
