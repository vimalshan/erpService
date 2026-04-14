using MassTransit;
using Microsoft.Extensions.Logging;

namespace WMTransactional.Infrastructure.Messaging.Consumers;

public class SalesOrderStatusChangedConsumer : IConsumer<SalesOrderStatusChangedMessage>
{
    private readonly ILogger<SalesOrderStatusChangedConsumer> _logger;

    public SalesOrderStatusChangedConsumer(ILogger<SalesOrderStatusChangedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<SalesOrderStatusChangedMessage> context)
    {
        var msg = context.Message;
        _logger.LogInformation("Sales Order {SoNumber} status changed to {NewStatus}",
            msg.SoNumber, msg.NewStatus);
        return Task.CompletedTask;
    }
}
