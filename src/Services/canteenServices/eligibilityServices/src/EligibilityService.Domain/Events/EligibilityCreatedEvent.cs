using EligibilityService.Domain.Common;
using EligibilityService.Domain.Entities;

namespace EligibilityService.Domain.Events;

public sealed class EligibilityCreatedEvent : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public EligibilityMaster EligibilityMaster { get; }

    public EligibilityCreatedEvent(EligibilityMaster eligibilityMaster)
        => EligibilityMaster = eligibilityMaster;
}
