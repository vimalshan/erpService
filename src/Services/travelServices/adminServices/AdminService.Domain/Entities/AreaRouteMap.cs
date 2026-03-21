namespace AdminService.Domain.Entities;

/// <summary>
/// Represents the mapping between areas and routes
/// </summary>
public class AreaRouteMap : BaseEntity
{
    /// <summary>
    /// Route identifier
    /// </summary>
    public long RouteId { get; set; }

    /// <summary>
    /// Area identifier
    /// </summary>
    public long AreaId { get; set; }

    /// <summary>
    /// Navigation property to RouteMaster
    /// </summary>
    public RouteMaster? RouteMaster { get; set; }

    /// <summary>
    /// Navigation property to AreaMaster
    /// </summary>
    public AreaMaster? AreaMaster { get; set; }
}
