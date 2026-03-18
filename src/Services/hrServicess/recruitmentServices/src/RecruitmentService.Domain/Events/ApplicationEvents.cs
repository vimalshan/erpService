using RecruitmentService.Domain.Common;
using RecruitmentService.Domain.ValueObjects;

namespace RecruitmentService.Domain.Events;

public sealed class ApplicationSubmittedEvent : DomainEvent
{
    public ApplicationSubmittedEvent(decimal appId, decimal vacancyId)
    {
        AppId = appId;
        VacancyId = vacancyId;
    }
    public decimal AppId { get; }
    public decimal VacancyId { get; }
}

public sealed class ApplicationStatusChangedEvent : DomainEvent
{
    public ApplicationStatusChangedEvent(decimal appId, ApplicationStatus previous, ApplicationStatus current)
    {
        AppId = appId;
        PreviousStatus = previous;
        CurrentStatus = current;
    }
    public decimal AppId { get; }
    public ApplicationStatus PreviousStatus { get; }
    public ApplicationStatus CurrentStatus { get; }
}
