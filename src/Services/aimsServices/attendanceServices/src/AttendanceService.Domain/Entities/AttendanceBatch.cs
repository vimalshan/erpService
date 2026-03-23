using AttendanceService.Domain.Common;
using AttendanceService.Domain.Events;
using AttendanceService.Domain.ValueObjects;

namespace AttendanceService.Domain.Entities;

public class AttendanceBatch : BaseAggregateRoot
{
    public int BatchMonthFrom { get; private set; }
    public int BatchMonthTo { get; private set; }
    public int BatchYearFrom { get; private set; }
    public int BatchYearEnd { get; private set; }
    public BatchStatus BatchStatus { get; private set; } = default!;
    public long BatchCreatedBy { get; private set; }
    public DateTime BatchCreatedOn { get; private set; }
    public long BatchLastModifiedBy { get; private set; }
    public DateTime BatchLastModifiedOn { get; private set; }

    private readonly List<AttendanceSummary> _summaries = [];
    public IReadOnlyCollection<AttendanceSummary> Summaries => _summaries.AsReadOnly();

    private readonly List<AttendanceLopMain> _lopRecords = [];
    public IReadOnlyCollection<AttendanceLopMain> LopRecords => _lopRecords.AsReadOnly();

    private AttendanceBatch() { }

    public static AttendanceBatch Create(long id, int monthFrom, int monthTo, int yearFrom, int yearEnd, long createdBy)
    {
        var batch = new AttendanceBatch
        {
            Id = id,
            BatchMonthFrom = monthFrom,
            BatchMonthTo = monthTo,
            BatchYearFrom = yearFrom,
            BatchYearEnd = yearEnd,
            BatchStatus = BatchStatus.Pending,
            BatchCreatedBy = createdBy,
            BatchCreatedOn = DateTime.UtcNow,
            BatchLastModifiedBy = createdBy,
            BatchLastModifiedOn = DateTime.UtcNow
        };
        return batch;
    }

    public void Close(long modifiedBy)
    {
        BatchStatus = BatchStatus.Closed;
        BatchLastModifiedBy = modifiedBy;
        BatchLastModifiedOn = DateTime.UtcNow;
        AddDomainEvent(new AttendanceBatchProcessedEvent(Id, BatchMonthFrom, BatchYearFrom));
    }
}
