using EmployeePrideManagement.Domain.Entities;

namespace EmployeePrideManagement.Domain.Events;

public class PrideMomentUpdatedEvent : IDomainEvent
{
    public MomentPride PrideMoment { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public PrideMomentUpdatedEvent(MomentPride prideMoment)
    {
        PrideMoment = prideMoment;
    }
}
