using WorkOrderService.Domain.Common;
using WorkOrderService.Domain.Entities;

namespace WorkOrderService.Domain.Events;

public sealed class WorkOrderCreatedEvent : IDomainEvent
{
    public WorkOrder WorkOrder { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public WorkOrderCreatedEvent(WorkOrder workOrder)
    {
        WorkOrder = workOrder;
    }
}
