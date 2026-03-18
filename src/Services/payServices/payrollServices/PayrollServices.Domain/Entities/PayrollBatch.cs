namespace PayrollServices.Domain.Entities;

/// <summary>
/// Represents a payroll batch for monthly salary processing
/// Maps to PAYROLL_BATCH table
/// </summary>
public class PayrollBatch : BaseEntity
{
    public long BatchId { get; init; }
    public string BatchMonth { get; set; } = null!; // YYYY-MM format
    public BatchStatus Status { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public long? UpdatedBy { get; set; }

    // Navigation property
    public ICollection<PayrollTransaction> Transactions { get; init; } = new List<PayrollTransaction>();

    public static PayrollBatch Create(long batchId, string batchMonth, long createdBy)
    {
        var batch = new PayrollBatch
        {
            BatchId = batchId,
            BatchMonth = batchMonth,
            Status = BatchStatus.Processing,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        return batch;
    }

    public void MarkAsCompleted(long updatedBy)
    {
        Status = BatchStatus.Completed;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void MarkAsFailed(long updatedBy)
    {
        Status = BatchStatus.Failed;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void MarkAsRevoked(long updatedBy)
    {
        Status = BatchStatus.Revoked;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}

/// <summary>
/// Status of payroll batch
/// </summary>
public enum BatchStatus
{
    Processing = 'P',
    Completed = 'C',
    Failed = 'F',
    Revoked = 'R'
}
