using HealthTransaction.Domain.Common;
using HealthTransaction.Domain.Entities;

namespace HealthTransaction.Domain.Events;

public class PreEmploymentCheckupCreatedEvent : BaseEvent
{
    public PreEmploymentCheckupCreatedEvent(PreEmploymentCheckup checkup) => Checkup = checkup;
    public PreEmploymentCheckup Checkup { get; }
}
