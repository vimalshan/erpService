using FillingOperationService.Domain.Common;
using FillingOperationService.Domain.Entities;

namespace FillingOperationService.Domain.Events;

public sealed record DowntimeRecordedEvent(FpgDowntime Downtime) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
