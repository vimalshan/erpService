namespace TransactionService.Infrastructure.Messaging.Consumers;

using MassTransit;
using Microsoft.Extensions.Logging;
using TransactionService.Infrastructure.Messaging.Events;

public sealed class OrderCreatedConsumer : IConsumer<OrderCreatedIntegrationEvent>
{
    private readonly ILogger<OrderCreatedConsumer> _logger;

    public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation(
            "New order {OrderMainId} created for Vendor: {VendorId} at Location: {LocationId}",
            message.OrderMainId, message.VendorId, message.LocationId);

        // Background: notify vendor, update procurement dashboard
        return Task.CompletedTask;
    }
}
