using ErrorLoggingService.Infrastructure.Messaging.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ErrorLoggingService.Infrastructure.Messaging.Consumers;

public sealed class ErrorLogNotificationConsumer : IConsumer<ErrorLoggedMessage>
{
    private readonly ILogger<ErrorLogNotificationConsumer> _logger;

    public ErrorLogNotificationConsumer(ILogger<ErrorLogNotificationConsumer> logger)
    {
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ErrorLoggedMessage> context)
    {
        var msg = context.Message;
        _logger.LogInformation(
            "Received ErrorLoggedMessage: Id={Id}, SP={SP}, Ref={Ref}, Date={Date}",
            msg.ErrorLogId, msg.StoredProcedureName, msg.ErrorReference, msg.ErrorDate);

        // Extend here: send alerts, persist to external sink, trigger escalation, etc.
        await Task.CompletedTask;
    }
}
