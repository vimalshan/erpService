using MedicalVisit.Domain.Common;
using MedicalVisit.Domain.Entities;

namespace MedicalVisit.Domain.Events;

public class VisitSubRecordAddedEvent : IDomainEvent
{
    public VisitMainAggregate Visit { get; }
    public VisitSubRecord SubRecord { get; }
    public DateTime OccurredOn { get; }

    public VisitSubRecordAddedEvent(VisitMainAggregate visit, VisitSubRecord subRecord)
    {
        Visit = visit;
        SubRecord = subRecord;
        OccurredOn = DateTime.UtcNow;
    }
}
