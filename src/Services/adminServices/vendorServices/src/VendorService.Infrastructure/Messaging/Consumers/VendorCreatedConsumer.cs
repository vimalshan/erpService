using MassTransit;
using Microsoft.Extensions.Logging;

namespace VendorService.Infrastructure.Messaging.Consumers;

public sealed class VendorCreatedConsumer : IConsumer<VendorCreatedMessage>
{
    private readonly ILogger<VendorCreatedConsumer> _logger;

    public VendorCreatedConsumer(ILogger<VendorCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<VendorCreatedMessage> context)
    {
        var msg = context.Message;
        _logger.LogInformation(
            "Vendor created event received: VendorId={VendorId}, Name={Name}, LocationId={LocationId}",
            msg.VendorId, msg.VendorName, msg.LocationId);

        // TODO: Trigger downstream workflows (e.g., notify procurement, sync ERP)
        return Task.CompletedTask;
    }
}
