using TrainingDevelopment.Domain.Common;
using TrainingDevelopment.Domain.Entities;

namespace TrainingDevelopment.Domain.Events;

public sealed class TrainingDroppedEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public TrainingDetail Training { get; }

    public TrainingDroppedEvent(TrainingDetail training)
    {
        Training = training;
    }
}
