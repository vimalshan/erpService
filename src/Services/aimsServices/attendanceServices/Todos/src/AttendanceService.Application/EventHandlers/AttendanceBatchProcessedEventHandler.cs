using AttendanceService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceService.Application.EventHandlers;

public class AttendanceBatchProcessedEventHandler(ILogger<AttendanceBatchProcessedEventHandler> logger)
    : INotificationHandler<AttendanceBatchProcessedEvent>
{
    public Task Handle(AttendanceBatchProcessedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Batch processed — BatchId={BatchId}, Month={Month}/{Year}",
            notification.BatchId, notification.Month, notification.Year);
        return Task.CompletedTask;
    }
}
