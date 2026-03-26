using AimsTransactionService.Domain.Common;
using AimsTransactionService.Domain.Events;

namespace AimsTransactionService.Domain.Aggregates;

public class CompOffAggregate : AggregateRoot
{
    public long EmployeeSysId { get; private set; }
    public decimal HoursRequested { get; private set; }
    public char Status { get; private set; }
    public DateTime RequestedOn { get; private set; }
    public long RequestedBy { get; private set; }
    public DateTime? ApprovedOn { get; private set; }
    public long? ApprovedBy { get; private set; }

    private CompOffAggregate() { }

    public static CompOffAggregate Request(
        long id,
        long employeeSysId,
        decimal hoursRequested,
        long requestedBy)
    {
        var compOff = new CompOffAggregate
        {
            Id = id,
            EmployeeSysId = employeeSysId,
            HoursRequested = hoursRequested,
            Status = 'P',
            RequestedOn = DateTime.UtcNow,
            RequestedBy = requestedBy
        };

        compOff.AddDomainEvent(new CompOffRequestedEvent(id, employeeSysId, hoursRequested));
        return compOff;
    }

    public void Approve(long approvedBy)
    {
        Status = 'A';
        ApprovedOn = DateTime.UtcNow;
        ApprovedBy = approvedBy;
    }

    public void Reject(long rejectedBy)
    {
        Status = 'R';
        ApprovedOn = DateTime.UtcNow;
        ApprovedBy = rejectedBy;
    }
}
