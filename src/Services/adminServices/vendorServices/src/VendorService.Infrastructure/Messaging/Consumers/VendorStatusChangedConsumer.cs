using MassTransit;
using Microsoft.Extensions.Logging;

namespace VendorService.Infrastructure.Messaging.Consumers;

public sealed class VendorStatusChangedConsumer : IConsumer<VendorStatusChangedMessage>
{
    private readonly ILogger<VendorStatusChangedConsumer> _logger;

    public VendorStatusChangedConsumer(ILogger<VendorStatusChangedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<VendorStatusChangedMessage> context)
    {
        var msg = context.Message;
        _logger.LogInformation(
            "Vendor status changed: VendorId={VendorId}, NewStatus={NewStatus}",
            msg.VendorId, msg.NewStatus);

        // TODO: Notify dependent services about vendor status change
        return Task.CompletedTask;
    }
}
