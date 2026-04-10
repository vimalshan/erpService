using SparshTransactional.Domain.Common;
using SparshTransactional.Domain.Events;

namespace SparshTransactional.Domain.Entities;

public class ScholarshipApplication : BaseEntity
{
    public long ApplicationId { get; set; }
    public long StudentId { get; set; }
    public long ScholarshipId { get; set; }
    public DateTime ApplicationDate { get; set; }
    public decimal? FamilyIncome { get; set; }
    public string ApplicationStatus { get; set; } = "S";
    public decimal? ApprovedAmount { get; set; }
    public long? ApprovedBy { get; set; }
    public string? RejectionReason { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }

    // Navigation
    public ScholarshipMaster Scholarship { get; set; } = null!;
    public ICollection<ScholarshipDisbursement> Disbursements { get; set; } = [];

    public static ScholarshipApplication Submit(long studentId, long scholarshipId,
        decimal? familyIncome, long createdBy)
    {
        var application = new ScholarshipApplication
        {
            StudentId = studentId,
            ScholarshipId = scholarshipId,
            ApplicationDate = DateTime.UtcNow,
            FamilyIncome = familyIncome,
            ApplicationStatus = "S",
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };
        application.AddDomainEvent(new ApplicationSubmittedEvent(application));
        return application;
    }

    public void Approve(long approvedBy, decimal approvedAmount)
    {
        ApplicationStatus = "A";
        ApprovedBy = approvedBy;
        ApprovedAmount = approvedAmount;
        UpdatedOn = DateTime.UtcNow;
        AddDomainEvent(new ApplicationApprovedEvent(this, approvedBy));
    }

    public void Reject(long rejectedBy, string? reason)
    {
        ApplicationStatus = "R";
        ApprovedBy = rejectedBy;
        RejectionReason = reason;
        UpdatedOn = DateTime.UtcNow;
        AddDomainEvent(new ApplicationRejectedEvent(this, rejectedBy, reason));
    }
}
