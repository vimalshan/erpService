using MedicalVisit.Domain.Common;
using MedicalVisit.Domain.Entities;

namespace MedicalVisit.Domain.Events;

public class VisitCancelledEvent : IDomainEvent
{
    public VisitMainAggregate Visit { get; }
    public DateTime OccurredOn { get; }

    public VisitCancelledEvent(VisitMainAggregate visit)
    {
        Visit = visit;
        OccurredOn = DateTime.UtcNow;
    }
}
