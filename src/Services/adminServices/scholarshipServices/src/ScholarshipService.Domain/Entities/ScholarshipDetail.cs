using ScholarshipService.Domain.Common;

namespace ScholarshipService.Domain.Entities;

/// <summary>Yearly scholarship detail record (maps to SCHOLARSHIP_DETAIL).</summary>
public class ScholarshipDetail : BaseEntity, IAuditableEntity
{
    public long Id { get; private set; }              // SCHDET_ID
    public int MainId { get; private set; }           // SCHDET_MAINID (references SCH_ID INT)
    public int Year { get; private set; }             // SCHDET_YEAR
    public string MarksFile { get; private set; } = string.Empty; // SCHDET_MARKSFILE
    public string MarksStatus { get; private set; } = "S";        // SCHDET_MARKSTATUS
    public string PayStatus { get; private set; } = "S";          // SCHDET_PAYSTATUS
    public DateTime CreatedOn { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }
    public long? UpdatedBy { get; private set; }
    public DateTime? ApprovedOn { get; private set; }
    public long? ApprovedBy { get; private set; }
    public DateTime? PayApprovedOn { get; private set; }
    public long? PayApprovedBy { get; private set; }
    public DateTime? PayDate { get; private set; }
    public long? PayAmount { get; private set; }
    public DateTime? PayUpdatedOn { get; private set; }
    public long? PayUpdatedBy { get; private set; }

    protected ScholarshipDetail() { }

    public static ScholarshipDetail Create(long id, int mainId, int year, string marksFile, long createdBy)
    {
        return new ScholarshipDetail
        {
            Id = id,
            MainId = mainId,
            Year = year,
            MarksFile = marksFile,
            MarksStatus = "P",   // Pending on creation
            PayStatus = "S",     // Scheduled on creation
            CreatedOn = DateTime.UtcNow,
            CreatedBy = createdBy
        };
    }

    public void ApproveMarks(long approvedBy)
    {
        MarksStatus = "A";
        ApprovedOn = DateTime.UtcNow;
        ApprovedBy = approvedBy;
        UpdatedOn = DateTime.UtcNow;
        UpdatedBy = approvedBy;
    }

    public void RejectMarks(long rejectedBy)
    {
        MarksStatus = "R";
        UpdatedOn = DateTime.UtcNow;
        UpdatedBy = rejectedBy;
    }

    public void ProcessPayment(long payAmount, long processedBy)
    {
        PayAmount = payAmount;
        PayStatus = "C";
        PayDate = DateTime.UtcNow;
        PayUpdatedOn = DateTime.UtcNow;
        PayUpdatedBy = processedBy;
    }

    public void UpdateMarksFile(string marksFile, long updatedBy)
    {
        MarksFile = marksFile;
        MarksStatus = "P";  // Reset to pending when file is updated
        UpdatedOn = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }
}
