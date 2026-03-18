using TdsService.Domain.Common;

namespace TdsService.Domain.Events;

public sealed record TdsFileUploadedEvent(
    long FileId,
    string FileName,
    string? PanNo) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
