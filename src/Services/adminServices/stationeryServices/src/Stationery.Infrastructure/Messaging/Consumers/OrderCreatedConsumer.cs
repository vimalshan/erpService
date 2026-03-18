using MassTransit;
using Microsoft.Extensions.Logging;
using Stationery.Domain.Events;

namespace Stationery.Infrastructure.Messaging.Consumers;

public class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly ILogger<OrderCreatedConsumer> _logger;

    public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation(
            "Order created: OrderId={OrderId}, VendorId={VendorId}, LocationId={LocationId}, Items={ItemCount}",
            message.OrderId, message.VendorId, message.LocationId, message.ItemCount);

        // Notify vendor, update procurement dashboard, etc.
        await Task.CompletedTask;
    }
}
