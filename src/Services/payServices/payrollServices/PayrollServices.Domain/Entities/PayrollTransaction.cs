namespace PayrollServices.Domain.Entities;

/// <summary>
/// Represents a payroll transaction for an employee
/// Maps to PAY_TRANDET table
/// </summary>
public class PayrollTransaction : BaseEntity
{
    public long TransactionId { get; init; }
    public long EmployeeSystemId { get; set; }
    public long BatchId { get; set; }
    public string Month { get; set; } = null!; // YYYY-MM format
    public decimal GrossSalary { get; set; }
    public decimal Deductions { get; set; }
    public decimal NetSalary { get; set; }
    public TransactionStatus Status { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public long? UpdatedBy { get; set; }

    // Navigation property
    public virtual PayrollBatch? Batch { get; set; }

    public static PayrollTransaction Create(
        long transactionId,
        long employeeSystemId,
        long batchId,
        string month,
        decimal grossSalary,
        decimal deductions,
        decimal netSalary,
        long createdBy)
    {
        var transaction = new PayrollTransaction
        {
            TransactionId = transactionId,
            EmployeeSystemId = employeeSystemId,
            BatchId = batchId,
            Month = month,
            GrossSalary = grossSalary,
            Deductions = deductions,
            NetSalary = netSalary,
            Status = TransactionStatus.Pending,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        return transaction;
    }

    public void MarkAsProcessed(long updatedBy)
    {
        Status = TransactionStatus.Processed;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void MarkAsDisburse(long updatedBy)
    {
        Status = TransactionStatus.Disbursed;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void MarkAsRejected(long updatedBy)
    {
        Status = TransactionStatus.Rejected;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void MarkAsVoid(long updatedBy)
    {
        Status = TransactionStatus.Void;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}

/// <summary>
/// Status of payroll transaction
/// </summary>
public enum TransactionStatus
{
    Pending = 'P',
    Processed = 'R',
    Disbursed = 'D',
    Rejected = 'X',
    Void = 'V'
}
