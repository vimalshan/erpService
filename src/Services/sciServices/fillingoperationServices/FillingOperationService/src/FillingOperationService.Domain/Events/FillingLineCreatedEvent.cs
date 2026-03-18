using FillingOperationService.Domain.Common;
using FillingOperationService.Domain.Entities;

namespace FillingOperationService.Domain.Events;

public sealed record FillingLineCreatedEvent(FillingLine FillingLine) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
