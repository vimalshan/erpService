using MassTransit;
using Microsoft.Extensions.Logging;

namespace SalesOrderService.Infrastructure.Messaging.Consumers;

/// <summary>
/// Listens for external "OrderShipped" events from other services.
/// Replace the message type with the actual contract from a shared contracts package.
/// </summary>
public sealed class OrderShippedConsumer(ILogger<OrderShippedConsumer> logger)
    : IConsumer<OrderShippedMessage>
{
    public Task Consume(ConsumeContext<OrderShippedMessage> context)
    {
        logger.LogInformation("Received OrderShipped: {SoNumber} shipped on {ShippedDate}",
            context.Message.SoNumber, context.Message.ShippedDate);
        // TODO: update QuantityShipped on order lines
        return Task.CompletedTask;
    }
}

public sealed record OrderShippedMessage(string SoNumber, DateTime ShippedDate, string? TrackingNumber);
