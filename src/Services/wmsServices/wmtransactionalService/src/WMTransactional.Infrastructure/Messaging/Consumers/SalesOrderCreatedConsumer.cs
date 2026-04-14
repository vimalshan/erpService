using MassTransit;
using Microsoft.Extensions.Logging;

namespace WMTransactional.Infrastructure.Messaging.Consumers;

public class SalesOrderCreatedConsumer : IConsumer<SalesOrderCreatedMessage>
{
    private readonly ILogger<SalesOrderCreatedConsumer> _logger;

    public SalesOrderCreatedConsumer(ILogger<SalesOrderCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<SalesOrderCreatedMessage> context)
    {
        var msg = context.Message;
        _logger.LogInformation("Sales Order created: {SoNumber} for Customer {CustomerId}", msg.SoNumber, msg.CustomerId);
        return Task.CompletedTask;
    }
}
