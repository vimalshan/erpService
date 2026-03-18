using TdsService.Domain.Common;

namespace TdsService.Domain.Events;

public sealed record TdsFileEmailSentEvent(
    long FileId,
    string? PanNo) : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}
