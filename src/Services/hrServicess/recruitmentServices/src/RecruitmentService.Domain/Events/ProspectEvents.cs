using RecruitmentService.Domain.Common;

namespace RecruitmentService.Domain.Events;

public sealed class ProspectRegisteredEvent : DomainEvent
{
    public ProspectRegisteredEvent(decimal userId, string? emailId)
    {
        UserId = userId;
        EmailId = emailId;
    }
    public decimal UserId { get; }
    public string? EmailId { get; }
}

public sealed class ProspectDeactivatedEvent : DomainEvent
{
    public ProspectDeactivatedEvent(decimal userId) => UserId = userId;
    public decimal UserId { get; }
}
