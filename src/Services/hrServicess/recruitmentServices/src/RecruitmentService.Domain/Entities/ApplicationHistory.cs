using RecruitmentService.Domain.Common;
using RecruitmentService.Domain.Events;
using RecruitmentService.Domain.ValueObjects;

namespace RecruitmentService.Domain.Entities;

public class ApplicationHistory : AggregateRoot
{
    public decimal AppId { get; private set; }
    public decimal AppSl { get; private set; }
    public string? AppUnit { get; private set; }
    public decimal? AppVacancyId { get; private set; }
    public ApplicationStatus Status { get; private set; } = ApplicationStatus.Received;
    public string? Remarks { get; private set; }
    public decimal? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    // Navigation
    public ICollection<ApplicationQualification> Qualifications { get; private set; } = new List<ApplicationQualification>();
    public ICollection<ApplicationTraining> Trainings { get; private set; } = new List<ApplicationTraining>();

    private ApplicationHistory() { }

    public static ApplicationHistory Submit(decimal appId, decimal sl, string? unit, decimal vacancyId, decimal submittedBy)
    {
        var app = new ApplicationHistory
        {
            AppId = appId,
            AppSl = sl,
            AppUnit = unit,
            AppVacancyId = vacancyId,
            Status = ApplicationStatus.Received,
            UpdatedBy = submittedBy,
            UpdatedOn = DateTime.UtcNow
        };

        app.AddDomainEvent(new ApplicationSubmittedEvent(appId, vacancyId));
        return app;
    }

    public void UpdateStatus(ApplicationStatus newStatus, string? remarks, decimal updatedBy)
    {
        var previous = Status;
        Status = newStatus;
        Remarks = remarks;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
        AddDomainEvent(new ApplicationStatusChangedEvent(AppId, previous, newStatus));
    }

    public void AddQualification(ApplicationQualification qualification)
        => Qualifications.Add(qualification);

    public void AddTraining(ApplicationTraining training)
        => Trainings.Add(training);
}
