using TravelRequestService.Domain.Common;

namespace TravelRequestService.Domain.Events;

public sealed class TravelRequestCancelledEvent : IDomainEvent
{
    public long PlanNumber { get; }
    public string CompanyCode { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public TravelRequestCancelledEvent(long planNumber, string companyCode)
    {
        PlanNumber = planNumber;
        CompanyCode = companyCode;
    }
}
