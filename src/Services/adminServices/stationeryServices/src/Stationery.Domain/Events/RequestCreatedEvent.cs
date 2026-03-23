using Stationery.Domain.Common;
using Stationery.Domain.Entities;

namespace Stationery.Domain.Events;

public class RequestCreatedEvent : DomainEvent
{
    public long RequestId { get; init; }
    public long RequestedBy { get; init; }
    public long? LocationId { get; init; }
    public string? UnitCode { get; init; }
    public int DetailCount { get; init; }

    private RequestCreatedEvent() { }

    public RequestCreatedEvent(RequestMain request)
    {
        RequestId = request.Id;
        RequestedBy = request.RequestedBy;
        LocationId = request.LocationId;
        UnitCode = request.UnitCode;
        DetailCount = request.Details.Count;
    }
}
