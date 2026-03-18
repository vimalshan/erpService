using WorkOrderService.Domain.Common;
using WorkOrderService.Domain.ValueObjects;

namespace WorkOrderService.Domain.Events;

public sealed class WorkOrderStatusChangedEvent : IDomainEvent
{
    public long WorkOrderId { get; }
    public WorkOrderStatus NewStatus { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public WorkOrderStatusChangedEvent(long workOrderId, WorkOrderStatus newStatus)
    {
        WorkOrderId = workOrderId;
        NewStatus = newStatus;
    }
}
