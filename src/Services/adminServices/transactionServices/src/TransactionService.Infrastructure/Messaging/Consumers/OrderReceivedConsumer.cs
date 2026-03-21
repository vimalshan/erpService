namespace TransactionService.Infrastructure.Messaging.Consumers;

using MassTransit;
using Microsoft.Extensions.Logging;
using TransactionService.Infrastructure.Messaging.Events;

public sealed class OrderReceivedConsumer : IConsumer<OrderReceivedIntegrationEvent>
{
    private readonly ILogger<OrderReceivedConsumer> _logger;

    public OrderReceivedConsumer(ILogger<OrderReceivedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<OrderReceivedIntegrationEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation(
            "Order {OrderMainId} sub {OrderSubId} received: {ReceivedQty} items by Employee: {ReceivedBy}",
            message.OrderMainId, message.OrderSubId, message.ReceivedQty, message.ReceivedBy);

        // Background: update inventory, reconcile budget, send receipt confirmation
        return Task.CompletedTask;
    }
}
