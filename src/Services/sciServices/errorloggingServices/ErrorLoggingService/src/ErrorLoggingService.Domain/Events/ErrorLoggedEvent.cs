using ErrorLoggingService.Domain.Entities;
using ErrorLoggingService.Domain.Events;

namespace ErrorLoggingService.Domain.Events;

public sealed class ErrorLoggedEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public ErrorLog ErrorLog { get; }

    public ErrorLoggedEvent(ErrorLog errorLog)
    {
        ErrorLog = errorLog;
    }
}
