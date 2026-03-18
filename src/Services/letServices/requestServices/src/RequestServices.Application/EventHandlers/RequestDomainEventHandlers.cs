using MediatR;
using Microsoft.Extensions.Logging;
using RequestServices.Domain.Events;

namespace RequestServices.Application.EventHandlers;

public class RequestCreatedEventHandler(ILogger<RequestCreatedEventHandler> logger)
    : INotificationHandler<RequestCreatedDomainNotification>
{
    public Task Handle(RequestCreatedDomainNotification notification, CancellationToken ct)
    {
        logger.LogInformation(
            "Domain event: RequestCreated — RequestId={RequestId}, Employee={Employee}, Supervisor={Supervisor}",
            notification.Event.RequestId,
            notification.Event.EmployeeUser,
            notification.Event.SupervisorUser);

        return Task.CompletedTask;
    }
}

public class RequestApprovedEventHandler(ILogger<RequestApprovedEventHandler> logger)
    : INotificationHandler<RequestApprovedDomainNotification>
{
    public Task Handle(RequestApprovedDomainNotification notification, CancellationToken ct)
    {
        logger.LogInformation(
            "Domain event: RequestApproved — RequestId={RequestId}, SerialNo={Serial}, ApprovedBy={User}",
            notification.Event.RequestId,
            notification.Event.SerialNumber,
            notification.Event.ApprovalUser);

        return Task.CompletedTask;
    }
}

public class RequestCancelledEventHandler(ILogger<RequestCancelledEventHandler> logger)
    : INotificationHandler<RequestCancelledDomainNotification>
{
    public Task Handle(RequestCancelledDomainNotification notification, CancellationToken ct)
    {
        logger.LogInformation(
            "Domain event: RequestCancelled — RequestId={RequestId}, SerialNo={Serial}",
            notification.Event.RequestId,
            notification.Event.SerialNumber);

        return Task.CompletedTask;
    }
}

// Notification wrappers so MediatR INotification works with domain events
public record RequestCreatedDomainNotification (RequestCreatedEvent  Event) : INotification;
public record RequestApprovedDomainNotification(RequestApprovedEvent Event) : INotification;
public record RequestCancelledDomainNotification(RequestCancelledEvent Event) : INotification;
