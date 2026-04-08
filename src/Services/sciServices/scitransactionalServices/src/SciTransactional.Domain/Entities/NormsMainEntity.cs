using SciTransactional.Domain.Common;
using SciTransactional.Domain.Events;

namespace SciTransactional.Domain.Entities;

public sealed class NormsMainEntity : AggregateRoot<long>
{
    public DateTime EffectiveDate { get; private set; }
    public DateTime? ClosureDate { get; private set; }

    private readonly List<NormsMasterEntity> _details = [];
    public IReadOnlyCollection<NormsMasterEntity> Details => _details.AsReadOnly();

    private NormsMainEntity() { }

    public static NormsMainEntity Create(DateTime effectiveDate)
    {
        var entity = new NormsMainEntity
        {
            EffectiveDate = effectiveDate
        };
        entity.AddDomainEvent(new NormCreatedEvent(0, effectiveDate));
        return entity;
    }

    public void Close()
    {
        if (ClosureDate.HasValue)
            throw new InvalidOperationException($"Norm {Id} is already closed.");
        ClosureDate = DateTime.UtcNow;
        AddDomainEvent(new NormClosedEvent(Id, ClosureDate.Value));
    }

    public void AddDetail(NormsMasterEntity detail)
    {
        _details.Add(detail);
    }

    public bool IsActive => !ClosureDate.HasValue;
}
