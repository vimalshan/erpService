using MassTransit;
using Microsoft.Extensions.Logging;
using AimsTransactionService.Domain.Events;

namespace AimsTransactionService.Infrastructure.Consumers;

public class SwipeProcessingConsumer(ILogger<SwipeProcessingConsumer> logger)
    : IConsumer<SwipeRecordedEvent>
{
    public async Task Consume(ConsumeContext<SwipeRecordedEvent> context)
    {
        var @event = context.Message;
        logger.LogInformation(
            "Processing swipe record {SwipeId} for employee {EmployeeSysId} — PunchStatus: {PunchStatus}",
            @event.SwipeId, @event.EmployeeSysId, @event.PunchStatus);

        // Integration point: validate swipe, trigger attendance calculation
        await Task.CompletedTask;
    }
}
