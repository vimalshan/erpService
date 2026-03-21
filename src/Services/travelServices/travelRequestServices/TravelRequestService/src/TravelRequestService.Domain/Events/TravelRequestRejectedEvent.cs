using TravelRequestService.Domain.Common;

namespace TravelRequestService.Domain.Events;

public sealed class TravelRequestRejectedEvent : IDomainEvent
{
    public long PlanNumber { get; }
    public string CompanyCode { get; }
    public long RejectedBy { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public TravelRequestRejectedEvent(long planNumber, string companyCode, long rejectedBy)
    {
        PlanNumber = planNumber;
        CompanyCode = companyCode;
        RejectedBy = rejectedBy;
    }
}
