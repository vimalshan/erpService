using EligibilityService.Domain.Common;
using EligibilityService.Domain.Entities;

namespace EligibilityService.Domain.Events;

public sealed class EligibilityUpdatedEvent : IDomainEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public EligibilityMaster EligibilityMaster { get; }
    public long ModifiedUser { get; }

    public EligibilityUpdatedEvent(EligibilityMaster eligibilityMaster, long modifiedUser)
    {
        EligibilityMaster = eligibilityMaster;
        ModifiedUser = modifiedUser;
    }
}
