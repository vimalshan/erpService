using MassTransit;
using Microsoft.Extensions.Logging;

namespace WMTransactional.Infrastructure.Messaging.Consumers;

public class ShipmentShippedConsumer : IConsumer<ShipmentShippedMessage>
{
    private readonly ILogger<ShipmentShippedConsumer> _logger;

    public ShipmentShippedConsumer(ILogger<ShipmentShippedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<ShipmentShippedMessage> context)
    {
        var msg = context.Message;
        _logger.LogInformation("Shipment {ShipmentNumber} shipped for Sales Order {SoId}", msg.ShipmentNumber, msg.SoId);
        return Task.CompletedTask;
    }
}
