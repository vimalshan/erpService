using TaskTransactional.Application.Interfaces;
using TaskTransactional.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TaskTransactional.Infrastructure.Messaging.EventHandlers;

public class ComplaintCreatedEventHandler(IMessagePublisher publisher, ILogger<ComplaintCreatedEventHandler> logger)
    : INotificationHandler<ComplaintCreatedEvent>
{
    public async Task Handle(ComplaintCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Complaint Created - GroupId {GroupId}, Unit {UnitCode}", notification.GroupId, notification.UnitCode);
        await publisher.PublishAsync("complaint.exchange", "complaint.created", notification, ct);
    }
}

public class ComplaintUpdatedEventHandler(IMessagePublisher publisher, ILogger<ComplaintUpdatedEventHandler> logger)
    : INotificationHandler<ComplaintUpdatedEvent>
{
    public async Task Handle(ComplaintUpdatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Complaint Updated - GroupId {GroupId}", notification.GroupId);
        await publisher.PublishAsync("complaint.exchange", "complaint.updated", notification, ct);
    }
}

public class TicketCreatedEventHandler(IMessagePublisher publisher, ILogger<TicketCreatedEventHandler> logger)
    : INotificationHandler<TicketCreatedEvent>
{
    public async Task Handle(TicketCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Ticket Created - TicketNum {TicketNum}", notification.TicketNum);
        await publisher.PublishAsync("complaint.exchange", "ticket.created", notification, ct);
    }
}

public class TicketClosedEventHandler(IMessagePublisher publisher, ILogger<TicketClosedEventHandler> logger)
    : INotificationHandler<TicketClosedEvent>
{
    public async Task Handle(TicketClosedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Ticket Closed - TicketNum {TicketNum}", notification.TicketNum);
        await publisher.PublishAsync("complaint.exchange", "ticket.closed", notification, ct);
    }
}

public class ActionCreatedEventHandler(IMessagePublisher publisher, ILogger<ActionCreatedEventHandler> logger)
    : INotificationHandler<ActionCreatedEvent>
{
    public async Task Handle(ActionCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Action Created - ActionNum {ActionNum}", notification.ActionNum);
        await publisher.PublishAsync("complaint.exchange", "action.created", notification, ct);
    }
}

public class ActionUpdatedEventHandler(IMessagePublisher publisher, ILogger<ActionUpdatedEventHandler> logger)
    : INotificationHandler<ActionUpdatedEvent>
{
    public async Task Handle(ActionUpdatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Action Updated - ActionNum {ActionNum}, Level {Level}", notification.ActionNum, notification.ActionLevel);
        await publisher.PublishAsync("complaint.exchange", "action.updated", notification, ct);
    }
}
