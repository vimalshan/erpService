using MassTransit;
using Microsoft.Extensions.Logging;

namespace WMTransactional.Infrastructure.Messaging.Consumers;

public class ReceivingCompletedConsumer : IConsumer<ReceivingCompletedMessage>
{
    private readonly ILogger<ReceivingCompletedConsumer> _logger;

    public ReceivingCompletedConsumer(ILogger<ReceivingCompletedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<ReceivingCompletedMessage> context)
    {
        var msg = context.Message;
        _logger.LogInformation("Receiving {ReceivingNumber} completed for Purchase Order {PoId}", msg.ReceivingNumber, msg.PoId);
        return Task.CompletedTask;
    }
}
