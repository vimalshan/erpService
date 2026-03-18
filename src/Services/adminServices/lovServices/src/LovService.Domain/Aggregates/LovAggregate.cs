using LovService.Domain.Entities;
using LovService.Domain.Events;

namespace LovService.Domain.Aggregates;

/// <summary>
/// LovAggregate is the aggregate root for LOV-related operations (DDD pattern).
/// It encapsulates LovType and its associated LovMasters, and raises domain events.
/// </summary>
public class LovAggregate
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public LovType LovType { get; private set; }
    private readonly List<LovMaster> _lovMasters = [];
    public IReadOnlyCollection<LovMaster> LovMasters => _lovMasters.AsReadOnly();

    public LovAggregate(LovType lovType)
    {
        LovType = lovType;
    }

    public LovMaster AddLovMaster(long lovId, string lovName, long updatedBy)
    {
        var master = LovMaster.Create(lovId, LovType.LovTypeId, lovName, updatedBy);
        _lovMasters.Add(master);
        _domainEvents.Add(new LovMasterCreatedEvent(lovId, LovType.LovTypeId, lovName));
        return master;
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}
