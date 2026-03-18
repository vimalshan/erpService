using RecruitmentService.Domain.Common;
using RecruitmentService.Domain.Events;
using RecruitmentService.Domain.ValueObjects;

namespace RecruitmentService.Domain.Entities;

public class Vacancy : AggregateRoot
{
    public decimal VacancyId { get; private set; }
    public string VacancyUnit { get; private set; } = default!;
    public decimal VacancyGrade { get; private set; }
    public decimal VacancyPositionId { get; private set; }
    public string VacancyName { get; private set; } = default!;
    public string? VacancyReporting { get; private set; }
    public decimal VacancyLocation { get; private set; }
    public decimal VacancyProcess { get; private set; }
    public string VacancyAge { get; private set; } = default!;
    public string VacancyExperience { get; private set; } = default!;
    public string VacancyQualification { get; private set; } = default!;
    public string? VacancyNarration1 { get; private set; }
    public string? VacancyNarration2 { get; private set; }
    public string? VacancyNarration3 { get; private set; }
    public string? VacancyNarration4 { get; private set; }
    public string? VacancyAttachment { get; private set; }
    public DateTime? VacancyLastDate { get; private set; }
    public bool AdvertiseIntranet { get; private set; }
    public DateTime? IntranetFromDate { get; private set; }
    public DateTime? IntranetToDate { get; private set; }
    public bool AdvertiseInternet { get; private set; }
    public DateTime? InternetFromDate { get; private set; }
    public DateTime? InternetToDate { get; private set; }
    public decimal? PostedBy { get; private set; }
    public DateTime? PostedDate { get; private set; }
    public decimal? ModifiedBy { get; private set; }
    public DateTime? ModifiedDate { get; private set; }
    public VacancyStatus LiveStatus { get; private set; } = VacancyStatus.Open;
    public string? Remarks { get; private set; }
    public bool InternalReferralAllowed { get; private set; }
    public string? InternalReferralEmail { get; private set; }
    public decimal VacancyUnitId { get; private set; }
    public string? VacancyType { get; private set; }
    public string? GradeList { get; private set; }
    public string? GradeType { get; private set; }
    public decimal? NumberOfOpenings { get; private set; }
    public decimal? CtcFrom { get; private set; }
    public decimal? CtcTo { get; private set; }
    public string? Designation { get; private set; }
    public bool AllowDownloadForm { get; private set; }
    public string? ApplicationFormFileName { get; private set; }
    public bool AllowUploadResume { get; private set; }
    public DateTime? InternalReferralCloseDate { get; private set; }
    public bool DisabilityFlag { get; private set; }
    public string? DisabilityLimit { get; private set; }

    // Navigation
    public ICollection<ApplicationHistory> Applications { get; private set; } = new List<ApplicationHistory>();

    private Vacancy() { }

    public static Vacancy Create(
        decimal vacancyId, string unit, decimal grade, decimal positionId,
        string name, decimal location, decimal process,
        string ageCriteria, string experience, string qualification,
        decimal unitId, decimal postedBy)
    {
        var vacancy = new Vacancy
        {
            VacancyId = vacancyId,
            VacancyUnit = unit,
            VacancyGrade = grade,
            VacancyPositionId = positionId,
            VacancyName = name,
            VacancyLocation = location,
            VacancyProcess = process,
            VacancyAge = ageCriteria,
            VacancyExperience = experience,
            VacancyQualification = qualification,
            VacancyUnitId = unitId,
            PostedBy = postedBy,
            PostedDate = DateTime.UtcNow,
            LiveStatus = VacancyStatus.Open
        };

        vacancy.AddDomainEvent(new VacancyCreatedEvent(vacancy));
        return vacancy;
    }

    public void Close(decimal modifiedBy)
    {
        LiveStatus = VacancyStatus.Closed;
        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new VacancyClosedEvent(VacancyId));
    }

    public void UpdateDetails(
        string name, string ageCriteria, string experience, string qualification,
        string? narration1, string? narration2, string? narration3, string? narration4,
        DateTime? lastDate, decimal modifiedBy)
    {
        VacancyName = name;
        VacancyAge = ageCriteria;
        VacancyExperience = experience;
        VacancyQualification = qualification;
        VacancyNarration1 = narration1;
        VacancyNarration2 = narration2;
        VacancyNarration3 = narration3;
        VacancyNarration4 = narration4;
        VacancyLastDate = lastDate;
        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow;
    }

    public void SetAttachment(string fileName)
    {
        VacancyAttachment = fileName;
        AddDomainEvent(new VacancyAttachmentUpdatedEvent(VacancyId, fileName));
    }
}
