using AuditService.Domain.Common;

namespace AuditService.Domain.Events;

public sealed class GoodPracticeCreatedEvent : DomainEvent
{
    public GoodPracticeCreatedEvent(long practiceId, string title)
    {
        PracticeId = practiceId;
        Title = title;
    }

    public long PracticeId { get; }
    public string Title { get; }
}
