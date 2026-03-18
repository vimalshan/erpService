using MediatR;
using Microsoft.Extensions.Logging;
using WorkOrderService.Application.Interfaces;
using WorkOrderService.Domain.Events;

namespace WorkOrderService.API.EventHandlers;

public class WorkOrderCreatedEventHandler : INotificationHandler<WorkOrderCreatedEvent>
{
    private readonly ILogger<WorkOrderCreatedEventHandler> _logger;
    private readonly IMessagePublisher? _messagePublisher;

    public WorkOrderCreatedEventHandler(ILogger<WorkOrderCreatedEventHandler> logger, IMessagePublisher? messagePublisher = null)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(WorkOrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Work order created: {WorkOrderName} by employee {CreatedBy}",
            notification.WorkOrder.WorkOrderName, notification.WorkOrder.CreatedBy);

        if (_messagePublisher is not null)
        {
            await _messagePublisher.PublishAsync("workorder.events.created", new
            {
                notification.WorkOrder.WorkOrderId,
                notification.WorkOrder.WorkOrderName,
                notification.OccurredOn
            }, cancellationToken);
        }
    }
}

public class TaskCompletedEventHandler : INotificationHandler<TaskCompletedEvent>
{
    private readonly ILogger<TaskCompletedEventHandler> _logger;
    private readonly IMessagePublisher? _messagePublisher;

    public TaskCompletedEventHandler(ILogger<TaskCompletedEventHandler> logger, IMessagePublisher? messagePublisher = null)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(TaskCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Task completed: {TaskName} in work order {WorkOrderId}",
            notification.Task.TaskName, notification.WorkOrderId);

        if (_messagePublisher is not null)
        {
            await _messagePublisher.PublishAsync("workorder.events.task-completed", new
            {
                notification.Task.TaskId,
                notification.Task.TaskName,
                notification.WorkOrderId,
                notification.OccurredOn
            }, cancellationToken);
        }
    }
}

public class WorkOrderStatusChangedEventHandler : INotificationHandler<WorkOrderStatusChangedEvent>
{
    private readonly ILogger<WorkOrderStatusChangedEventHandler> _logger;
    private readonly IMessagePublisher? _messagePublisher;

    public WorkOrderStatusChangedEventHandler(ILogger<WorkOrderStatusChangedEventHandler> logger, IMessagePublisher? messagePublisher = null)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(WorkOrderStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Work order {WorkOrderId} status changed to {Status}",
            notification.WorkOrderId, notification.NewStatus);

        if (_messagePublisher is not null)
        {
            await _messagePublisher.PublishAsync("workorder.events.status-changed", new
            {
                notification.WorkOrderId,
                NewStatus = notification.NewStatus.Name,
                notification.OccurredOn
            }, cancellationToken);
        }
    }
}
