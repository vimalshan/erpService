using AttendanceService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceService.Application.EventHandlers;

public class SwipePunchRecordedEventHandler(ILogger<SwipePunchRecordedEventHandler> logger)
    : INotificationHandler<SwipePunchRecordedEvent>
{
    public Task Handle(SwipePunchRecordedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: SwipePunch recorded — EmpId={EmpId}, SwipeId={SwipeId}, Time={Time}",
            notification.EmpSysId, notification.SwipeId, notification.PunchTime);
        return Task.CompletedTask;
    }
}
