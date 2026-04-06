using PayTransactionalService.Domain.Common;

namespace PayTransactionalService.Domain.Entities;

/// <summary>
/// Payroll batch for grouping transactions (PAYROLL_BATCH reference)
/// </summary>
public sealed class PayrollBatch : AuditableEntity
{
    public long Id { get; set; }
    public string MonthYear { get; set; } = null!; // YYYY-MM
    public string Status { get; set; } = "P"; // P=Processing, C=Complete, R=Revoked
    public int TransactionCount { get; set; }
    public DateTime? CompletedAt { get; set; }

    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private PayrollBatch() { }

    public static PayrollBatch Create(string monthYear, string createdBy)
    {
        var batch = new PayrollBatch
        {
            MonthYear = monthYear,
            Status = "P",
            TransactionCount = 0,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };

        batch._domainEvents.Add(new PayrollBatchCreatedEvent(monthYear));
        return batch;
    }

    public void Complete(int transactionCount)
    {
        Status = "C";
        TransactionCount = transactionCount;
        CompletedAt = DateTime.UtcNow;
        ModifiedAt = DateTime.UtcNow;
        _domainEvents.Add(new PayrollBatchCompletedEvent(Id, MonthYear, transactionCount));
    }

    public void Revoke(string revokedBy)
    {
        if (Status == "R")
            throw new InvalidOperationException("Batch is already revoked");
        Status = "R";
        ModifiedAt = DateTime.UtcNow;
        ModifiedBy = revokedBy;
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}

public sealed class PayrollBatchCreatedEvent : DomainEvent
{
    public string MonthYear { get; }
    public PayrollBatchCreatedEvent(string monthYear) => MonthYear = monthYear;
}

public sealed class PayrollBatchCompletedEvent : DomainEvent
{
    public long BatchId { get; }
    public string MonthYear { get; }
    public int TransactionCount { get; }

    public PayrollBatchCompletedEvent(long batchId, string monthYear, int transactionCount)
    {
        BatchId = batchId;
        MonthYear = monthYear;
        TransactionCount = transactionCount;
    }
}
