using ScholarshipService.Domain.Common;
using ScholarshipService.Domain.Events;

namespace ScholarshipService.Domain.Entities;

/// <summary>
/// Aggregate root for scholarship applications (maps to SCHOLARSHIP_MAIN).
/// Encapsulates the lifecycle of a scholarship: creation, approval, stopping.
/// </summary>
public class ScholarshipMain : BaseEntity, IAuditableEntity
{
    public int Id { get; private set; }               // SCH_ID
    public int EmployeeSysId { get; private set; }    // SCH_EMPSYSID
    public int GradeId { get; private set; }          // SCH_GRADEID
    public int DependentId { get; private set; }      // SCH_DEPENDID
    public string ChildName { get; private set; } = string.Empty;
    public string LastSchool { get; private set; } = string.Empty;
    public decimal LastYearOfSchool { get; private set; }
    public string LastExam { get; private set; } = string.Empty;   // 10 or 12
    public string CgpaFlag { get; private set; } = "N";
    public decimal MarksPercentage { get; private set; }
    public decimal MarksGpa { get; private set; }
    public string MarksFile { get; private set; } = string.Empty;
    public string CourseName { get; private set; } = string.Empty;
    public int CourseJoinYear { get; private set; }
    public decimal CourseJoinMonth { get; private set; }
    public long CourseDuration { get; private set; }
    public string? AdmissionReceiptFile { get; private set; }
    public string? PaymentMode { get; private set; }
    public string? ChildAccountNumber { get; private set; }
    public string? ChildBankIfsc { get; private set; }
    public string? ChildBankMicr { get; private set; }
    public string? EntryStatus { get; private set; }    // E=Entered, A=Approved, N=Not Eligible, B=Returned
    public string Source { get; private set; } = string.Empty;
    public decimal DisbursementAmount { get; private set; }
    public string DisbursementFrequency { get; private set; } = string.Empty;
    public string LiveStatus { get; private set; } = "A";
    public DateTime CreatedOn { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }
    public long? UpdatedBy { get; private set; }
    public int ApprovalBy { get; private set; }
    public DateTime? ApprovalOn { get; private set; }
    public string ApprovalRemarks { get; private set; } = string.Empty;
    public string StopReason { get; private set; } = string.Empty;
    public DateTime? StopDate { get; private set; }
    public DateTime? StopEnteredOn { get; private set; }
    public int? StopEnteredBy { get; private set; }
    public string IsOffline { get; private set; } = "N";
    public int? OfflineYear { get; private set; }

    private readonly List<ScholarshipDetail> _details = new();
    public IReadOnlyCollection<ScholarshipDetail> Details => _details.AsReadOnly();

    protected ScholarshipMain() { }

    public static ScholarshipMain Create(
        int id, int employeeSysId, int gradeId, int dependentId, string childName,
        string lastSchool, decimal lastYearOfSchool, string lastExam, string cgpaFlag,
        decimal marksPercentage, decimal marksGpa, string marksFile, string courseName,
        int courseJoinYear, decimal courseJoinMonth, long courseDuration,
        string? admissionReceiptFile, string? paymentMode, string? childAccountNumber,
        string? childBankIfsc, string? childBankMicr, string source,
        decimal disbursementAmount, string disbursementFrequency,
        int createdBy, string isOffline = "N", int? offlineYear = null)
    {
        var scholarship = new ScholarshipMain
        {
            Id = id,
            EmployeeSysId = employeeSysId,
            GradeId = gradeId,
            DependentId = dependentId,
            ChildName = childName,
            LastSchool = lastSchool,
            LastYearOfSchool = lastYearOfSchool,
            LastExam = lastExam,
            CgpaFlag = cgpaFlag,
            MarksPercentage = marksPercentage,
            MarksGpa = marksGpa,
            MarksFile = marksFile,
            CourseName = courseName,
            CourseJoinYear = courseJoinYear,
            CourseJoinMonth = courseJoinMonth,
            CourseDuration = courseDuration,
            AdmissionReceiptFile = admissionReceiptFile,
            PaymentMode = paymentMode,
            ChildAccountNumber = childAccountNumber,
            ChildBankIfsc = childBankIfsc,
            ChildBankMicr = childBankMicr,
            EntryStatus = "E",
            Source = source,
            DisbursementAmount = disbursementAmount,
            DisbursementFrequency = disbursementFrequency,
            LiveStatus = "A",
            CreatedOn = DateTime.UtcNow,
            CreatedBy = createdBy,
            IsOffline = isOffline,
            OfflineYear = offlineYear
        };

        scholarship.AddDomainEvent(new ScholarshipCreatedEvent(id, employeeSysId, childName));
        return scholarship;
    }

    public void Approve(int approvedBy, string? remarks = null)
    {
        if (EntryStatus == "A")
            throw new InvalidOperationException("Scholarship is already approved.");

        EntryStatus = "A";
        ApprovalBy = approvedBy;
        ApprovalOn = DateTime.UtcNow;
        ApprovalRemarks = remarks ?? string.Empty;
        UpdatedOn = DateTime.UtcNow;
        UpdatedBy = approvedBy;

        AddDomainEvent(new ScholarshipApprovedEvent(Id, approvedBy));
    }

    public void Return(int updatedBy, string remarks)
    {
        EntryStatus = "B";
        ApprovalRemarks = remarks;
        UpdatedOn = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    public void MarkNotEligible(int updatedBy)
    {
        EntryStatus = "N";
        UpdatedOn = DateTime.UtcNow;
        UpdatedBy = updatedBy;
    }

    public void Stop(string reason, int stoppedBy)
    {
        LiveStatus = "S";
        StopReason = reason;
        StopDate = DateTime.UtcNow;
        StopEnteredOn = DateTime.UtcNow;
        StopEnteredBy = stoppedBy;
        UpdatedOn = DateTime.UtcNow;
        UpdatedBy = stoppedBy;

        AddDomainEvent(new ScholarshipStoppedEvent(Id, stoppedBy, reason));
    }

    public void AddDetail(ScholarshipDetail detail) => _details.Add(detail);
}
