using MedicalVisit.Domain.Common;
using MedicalVisit.Domain.Entities;

namespace MedicalVisit.Domain.Events;

public class VisitCreatedEvent : IDomainEvent
{
    public VisitMainAggregate Visit { get; }
    public DateTime OccurredOn { get; }

    public VisitCreatedEvent(VisitMainAggregate visit)
    {
        Visit = visit;
        OccurredOn = DateTime.UtcNow;
    }
}
