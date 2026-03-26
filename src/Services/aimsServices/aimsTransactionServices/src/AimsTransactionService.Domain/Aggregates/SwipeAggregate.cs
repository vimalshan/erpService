using AimsTransactionService.Domain.Common;
using AimsTransactionService.Domain.Enums;
using AimsTransactionService.Domain.Events;
using AimsTransactionService.Domain.ValueObjects;

namespace AimsTransactionService.Domain.Aggregates;

public class SwipeAggregate : AggregateRoot
{
    public long EmployeeSysId { get; private set; }
    public PunchInfo PunchInfo { get; private set; } = null!;
    public DateTime PunchTime { get; private set; }
    public PullStatus PullStatus { get; private set; }
    public long UpdatedBy { get; private set; }
    public DateTime UpdatedOn { get; private set; }

    private SwipeAggregate() { }

    public static SwipeAggregate Record(
        long id,
        long employeeSysId,
        int gateNo,
        DateTime punchTime,
        char punchStatus,
        int? machineNo,
        string? referenceNo,
        long updatedBy)
    {
        if (punchStatus is not ('I' or 'O'))
            throw new InvalidOperationException("Invalid punch status. Use I for In, O for Out.");

        var swipe = new SwipeAggregate
        {
            Id = id,
            EmployeeSysId = employeeSysId,
            PunchInfo = new PunchInfo(gateNo, (PunchStatus)punchStatus, machineNo, referenceNo),
            PunchTime = punchTime,
            PullStatus = PullStatus.Automatic,
            UpdatedBy = updatedBy,
            UpdatedOn = DateTime.UtcNow
        };

        swipe.AddDomainEvent(new SwipeRecordedEvent(id, employeeSysId, punchTime, punchStatus));
        return swipe;
    }

    public void MarkForManualReview()
    {
        PullStatus = PullStatus.Manual;
    }
}
