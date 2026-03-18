using Stationery.Domain.Common;
using Stationery.Domain.Entities;

namespace Stationery.Domain.Events;

public class RequestCreatedEvent : DomainEvent
{
    public RequestMain Request { get; init; } = null!;

    // For MassTransit
    private RequestCreatedEvent() { }

    public RequestCreatedEvent(RequestMain request)
    {
        Request = request;
    }
}
