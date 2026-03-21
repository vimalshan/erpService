using TravelRequestService.Domain.Common;

namespace TravelRequestService.Domain.Events;

public sealed class TravelRequestApprovedEvent : IDomainEvent
{
    public long PlanNumber { get; }
    public string CompanyCode { get; }
    public long ApprovedBy { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public TravelRequestApprovedEvent(long planNumber, string companyCode, long approvedBy)
    {
        PlanNumber = planNumber;
        CompanyCode = companyCode;
        ApprovedBy = approvedBy;
    }
}
