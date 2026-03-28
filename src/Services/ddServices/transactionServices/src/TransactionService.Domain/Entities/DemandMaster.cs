namespace TransactionService.Domain.Entities;

public class DemandMaster : BaseEntity
{
    public string DemandType { get; set; } = string.Empty;
    public long DepartmentId { get; set; }
    public string DemandDescription { get; set; } = string.Empty;
    public DateTime RequiredDate { get; set; }
    public string Priority { get; set; } = string.Empty;
    public char DemandStatus { get; set; } = 'O';
    public long CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public string? ApprovalRemarks { get; set; }
    public long? ApprovedBy { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public string? CompletionRemarks { get; set; }
    public long? CompletedBy { get; set; }
    public DateTime? CompletionDate { get; set; }

    public DemandMaster() { }

    public DemandMaster(string demandType, long departmentId, string description, DateTime requiredDate, string priority, long createdBy)
    {
        DemandType = demandType;
        DepartmentId = departmentId;
        DemandDescription = description;
        RequiredDate = requiredDate;
        Priority = priority;
        CreatedBy = createdBy;
        DemandStatus = 'O';
        CreatedOn = DateTime.UtcNow;
    }

    public bool IsOpen => DemandStatus == 'O';
    public bool IsApproved => DemandStatus == 'A';
    public bool IsRejected => DemandStatus == 'R';
    public bool IsCompleted => DemandStatus == 'C';
}
