using BusServices.Domain.Common;
using BusServices.Domain.Exceptions;
using BusServices.Domain.ValueObjects;

namespace BusServices.Domain.Entities;

/// <summary>Maps to BUSROUTE_MASTER table.</summary>
public sealed class BusRoute : BaseEntity
{
    public int RouteId { get; private set; }
    public int BusId { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public RouteStatus Status { get; private set; } = null!;
    public long LastModifiedBy { get; private set; }
    public DateTime LastModifiedOn { get; private set; }

    private BusRoute() { }

    internal static BusRoute Create(int routeId, int busId, string name, string? description, long createdBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Route name cannot be empty.");
        if (name.Length > 100)
            throw new DomainException("Route name cannot exceed 100 characters.");

        return new BusRoute
        {
            RouteId = routeId,
            BusId = busId,
            Name = name.Trim(),
            Description = description,
            Status = RouteStatus.Active,
            LastModifiedBy = createdBy,
            LastModifiedOn = DateTime.UtcNow
        };
    }

    public void Deactivate(long modifiedBy)
    {
        Status = RouteStatus.Inactive;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }

    public void Suspend(long modifiedBy)
    {
        Status = RouteStatus.Suspended;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
