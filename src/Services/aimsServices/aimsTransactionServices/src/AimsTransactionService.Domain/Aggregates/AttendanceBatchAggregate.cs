using AimsTransactionService.Domain.Common;
using AimsTransactionService.Domain.Entities;
using AimsTransactionService.Domain.Enums;
using AimsTransactionService.Domain.Events;

namespace AimsTransactionService.Domain.Aggregates;

public class AttendanceBatchAggregate : AggregateRoot
{
    public DateTime MonthStart { get; private set; }
    public DateTime MonthEnd { get; private set; }
    public BatchStatus Status { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }

    private readonly List<AttendanceLopMain> _lopRecords = [];
    public IReadOnlyCollection<AttendanceLopMain> LopRecords => _lopRecords.AsReadOnly();

    private AttendanceBatchAggregate() { }

    public static AttendanceBatchAggregate Create(
        long id,
        DateTime monthStart,
        DateTime monthEnd,
        long createdBy)
    {
        return new AttendanceBatchAggregate
        {
            Id = id,
            MonthStart = monthStart,
            MonthEnd = monthEnd,
            Status = BatchStatus.New,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };
    }

    public void AddLopRecord(AttendanceLopMain lopRecord)
    {
        _lopRecords.Add(lopRecord);
    }

    public void MarkCompleted(int employeesProcessed)
    {
        Status = BatchStatus.Completed;
        AddDomainEvent(new AttendanceBatchProcessedEvent(Id, MonthStart, MonthEnd, employeesProcessed));
    }

    public void MarkProcessing()
    {
        Status = BatchStatus.Processing;
    }

    public void HydrateLopRecords(IEnumerable<AttendanceLopMain> records)
    {
        _lopRecords.Clear();
        _lopRecords.AddRange(records);
    }
}
