using EmployeePrideManagement.Domain.Entities;

namespace EmployeePrideManagement.Domain.Events;

public class PrideMomentCreatedEvent : IDomainEvent
{
    public MomentPride PrideMoment { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public PrideMomentCreatedEvent(MomentPride prideMoment)
    {
        PrideMoment = prideMoment;
    }
}
