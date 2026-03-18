using WorkOrderService.Domain.Common;
using WorkOrderService.Domain.Entities;

namespace WorkOrderService.Domain.Events;

public sealed class TaskAssignedEvent : IDomainEvent
{
    public long WorkOrderId { get; }
    public WorkTask Task { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public TaskAssignedEvent(long workOrderId, WorkTask task)
    {
        WorkOrderId = workOrderId;
        Task = task;
    }
}
