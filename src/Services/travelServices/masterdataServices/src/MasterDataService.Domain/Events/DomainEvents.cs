using MasterDataService.Domain.Common;
using MasterDataService.Domain.Entities;

namespace MasterDataService.Domain.Events;

public sealed class GuestHouseCreatedEvent : IDomainEvent
{
    public GuestHouse GuestHouse { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public GuestHouseCreatedEvent(GuestHouse guestHouse) => GuestHouse = guestHouse;
}

public sealed class GuestHouseUpdatedEvent : IDomainEvent
{
    public GuestHouse GuestHouse { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public GuestHouseUpdatedEvent(GuestHouse guestHouse) => GuestHouse = guestHouse;
}

public sealed class AreaCreatedEvent : IDomainEvent
{
    public Area Area { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public AreaCreatedEvent(Area area) => Area = area;
}

public sealed class RouteCreatedEvent : IDomainEvent
{
    public Route Route { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public RouteCreatedEvent(Route route) => Route = route;
}
