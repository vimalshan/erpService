using Recruitment.Domain.Common;
using Recruitment.Domain.Enums;
using Recruitment.Domain.ValueObjects;

namespace Recruitment.Domain.Entities;

/// <summary>
/// Application entity representing a job application
/// </summary>
public class Application : Entity
{
    public decimal ApplicationNumber { get; private set; }
    public decimal JobId { get; private set; }
    public ContactInfo ContactInfo { get; private set; }
    public string CurrentJobDesciption { get; private set; }
    public string Achievements { get; private set; }
    public string ReasonForJoining { get; private set; }
    public string Strength { get; private set; }
    public string Awards { get; private set; }
    public decimal? CrtMarks { get; private set; }
    public decimal? DomainMarks { get; private set; }
    public string CrtDocumentPath { get; private set; }
    public string DomainDocumentPath { get; private set; }
    public ApplicationStatus Status { get; private set; }
    public List<ApplicationStatusHistory> StatusHistories { get; private set; } = new();
    public List<CourseDetail> CourseDetails { get; private set; } = new();

    // Required for EF Core
    public Application()
    {
        CurrentJobDesciption = string.Empty;
        Achievements = string.Empty;
        ReasonForJoining = string.Empty;
        Strength = string.Empty;
        Awards = string.Empty;
        CrtDocumentPath = string.Empty;
        DomainDocumentPath = string.Empty;
    }

    public Application(
        decimal applicationNumber,
        decimal jobId,
        ContactInfo contactInfo)
    {
        ApplicationNumber = applicationNumber;
        JobId = jobId;
        ContactInfo = contactInfo;
        Status = ApplicationStatus.Pending;
        Id = applicationNumber;
        CurrentJobDesciption = string.Empty;
        Achievements = string.Empty;
        ReasonForJoining = string.Empty;
        Strength = string.Empty;
        Awards = string.Empty;
        CrtDocumentPath = string.Empty;
        DomainDocumentPath = string.Empty;
    }

    public void UpdateApplicationDetails(
        string currentJobDescription,
        string achievements,
        string reasonForJoining,
        string strength,
        string awards)
    {
        CurrentJobDesciption = currentJobDescription;
        Achievements = achievements;
        ReasonForJoining = reasonForJoining;
        Strength = strength;
        Awards = awards;
        ModifiedDate = DateTime.UtcNow;
    }

    public void SetMarks(decimal crtMarks, decimal domainMarks)
    {
        CrtMarks = crtMarks;
        DomainMarks = domainMarks;
        ModifiedDate = DateTime.UtcNow;
    }

    public void SetDocuments(string crtPath, string domainPath)
    {
        CrtDocumentPath = crtPath;
        DomainDocumentPath = domainPath;
        ModifiedDate = DateTime.UtcNow;
    }

    public void ChangeStatus(ApplicationStatus newStatus, string remark, string updatedBy)
    {
        Status = newStatus;
        StatusHistories.Add(new ApplicationStatusHistory(ApplicationNumber, newStatus, remark, updatedBy));
        ModifiedDate = DateTime.UtcNow;
        ModifiedBy = updatedBy;
    }

    public void AddCourseDetail(CourseDetail courseDetail)
    {
        CourseDetails.Add(courseDetail);
    }
}
