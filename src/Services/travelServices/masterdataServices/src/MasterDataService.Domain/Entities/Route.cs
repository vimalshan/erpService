using MasterDataService.Domain.Common;
using MasterDataService.Domain.Events;

namespace MasterDataService.Domain.Entities;

public class Route : AuditableEntity
{
    public int RouteId { get; private set; }
    public string RouteName { get; private set; } = string.Empty;

    private Route() { }

    public Route(int routeId, string routeName)
    {
        RouteId = routeId;
        RouteName = routeName ?? throw new ArgumentNullException(nameof(routeName));
        AddDomainEvent(new RouteCreatedEvent(this));
    }

    public void UpdateName(string name)
    {
        RouteName = name ?? throw new ArgumentNullException(nameof(name));
    }
}
