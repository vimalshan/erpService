using SparshTransactional.Domain.Common;
using SparshTransactional.Domain.Events;

namespace SparshTransactional.Domain.Entities;

public class ScholarshipDisbursement : BaseEntity
{
    public long DisbursementId { get; set; }
    public long ApplicationId { get; set; }
    public long StudentId { get; set; }
    public long ScholarshipId { get; set; }
    public decimal DisbursementAmount { get; set; }
    public DateTime? DisbursementDate { get; set; }
    public string DisbursementStatus { get; set; } = "P";
    public string? PaymentReference { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }

    // Navigation
    public ScholarshipApplication Application { get; set; } = null!;

    public static ScholarshipDisbursement Create(long applicationId, long studentId,
        long scholarshipId, decimal amount, long createdBy)
    {
        var disbursement = new ScholarshipDisbursement
        {
            ApplicationId = applicationId,
            StudentId = studentId,
            ScholarshipId = scholarshipId,
            DisbursementAmount = amount,
            DisbursementStatus = "P",
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };
        disbursement.AddDomainEvent(new DisbursementCreatedEvent(disbursement));
        return disbursement;
    }

    public void Complete(string paymentReference)
    {
        DisbursementStatus = "C";
        DisbursementDate = DateTime.UtcNow;
        PaymentReference = paymentReference;
        UpdatedOn = DateTime.UtcNow;
        AddDomainEvent(new DisbursementCompletedEvent(this));
    }

    public void Fail()
    {
        DisbursementStatus = "F";
        UpdatedOn = DateTime.UtcNow;
    }
}
