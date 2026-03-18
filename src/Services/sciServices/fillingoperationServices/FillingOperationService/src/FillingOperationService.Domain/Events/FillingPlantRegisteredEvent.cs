using FillingOperationService.Domain.Common;
using FillingOperationService.Domain.Entities;

namespace FillingOperationService.Domain.Events;

public sealed record FillingPlantRegisteredEvent(FillingPlant Plant) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
