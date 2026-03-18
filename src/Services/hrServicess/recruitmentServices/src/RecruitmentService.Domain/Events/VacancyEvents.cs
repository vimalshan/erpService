using RecruitmentService.Domain.Common;
using RecruitmentService.Domain.Entities;

namespace RecruitmentService.Domain.Events;

public sealed class VacancyCreatedEvent : DomainEvent
{
    public VacancyCreatedEvent(Vacancy vacancy) => Vacancy = vacancy;
    public Vacancy Vacancy { get; }
}

public sealed class VacancyClosedEvent : DomainEvent
{
    public VacancyClosedEvent(decimal vacancyId) => VacancyId = vacancyId;
    public decimal VacancyId { get; }
}

public sealed class VacancyAttachmentUpdatedEvent : DomainEvent
{
    public VacancyAttachmentUpdatedEvent(decimal vacancyId, string fileName)
    {
        VacancyId = vacancyId;
        FileName = fileName;
    }
    public decimal VacancyId { get; }
    public string FileName { get; }
}
