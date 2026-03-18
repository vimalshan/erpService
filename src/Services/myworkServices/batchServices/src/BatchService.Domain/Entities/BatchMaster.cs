using BatchService.Domain.Common;
using BatchService.Domain.Events;
using BatchService.Domain.ValueObjects;

namespace BatchService.Domain.Entities;

/// <summary>
/// Aggregate root — maps to [BATCH_MASTER].
/// </summary>
public sealed class BatchMaster : BaseEntity
{
    // ── Persisted columns ────────────────────────────────────────────────
    public long BatchId           { get; private set; }
    public int  BatchMonthNo      { get; private set; }
    public char BatchStatusChar   { get; private set; }
    public long BatchLastModifiedBy { get; private set; }
    public DateTime BatchLastModifiedOn { get; private set; }

    // ── Value-object wrappers ─────────────────────────────────────────────
    public MonthNumber MonthNumber  => new(BatchMonthNo);
    public ValueObjects.BatchStatus Status => ValueObjects.BatchStatus.From(BatchStatusChar);

    // ── EF Core requires a parameterless constructor ─────────────────────
    private BatchMaster() { }

    // ── Factory method ────────────────────────────────────────────────────
    public static BatchMaster Create(long batchId, int monthNo, long modifiedBy)
    {
        var batch = new BatchMaster
        {
            BatchId             = batchId,
            BatchMonthNo        = new MonthNumber(monthNo).Value,
            BatchStatusChar     = ValueObjects.BatchStatus.Open.Value,
            BatchLastModifiedBy = modifiedBy,
            BatchLastModifiedOn = DateTime.UtcNow
        };

        batch.AddDomainEvent(new BatchCreatedEvent(batch.BatchId, batch.BatchMonthNo));
        return batch;
    }

    // ── Behaviour ─────────────────────────────────────────────────────────
    public void Close(long modifiedBy)
    {
        if (Status == ValueObjects.BatchStatus.Closed)
            throw new InvalidOperationException("Batch is already closed.");

        var previous = BatchStatusChar;
        BatchStatusChar     = ValueObjects.BatchStatus.Closed.Value;
        BatchLastModifiedBy = modifiedBy;
        BatchLastModifiedOn = DateTime.UtcNow;

        AddDomainEvent(new BatchStatusChangedEvent(BatchId, previous, BatchStatusChar));
    }

    public void Lock(long modifiedBy)
    {
        var previous = BatchStatusChar;
        BatchStatusChar     = ValueObjects.BatchStatus.Locked.Value;
        BatchLastModifiedBy = modifiedBy;
        BatchLastModifiedOn = DateTime.UtcNow;

        AddDomainEvent(new BatchStatusChangedEvent(BatchId, previous, BatchStatusChar));
    }

    public void UpdateMonth(int newMonthNo, long modifiedBy)
    {
        BatchMonthNo        = new MonthNumber(newMonthNo).Value;
        BatchLastModifiedBy = modifiedBy;
        BatchLastModifiedOn = DateTime.UtcNow;
    }
}
