namespace AdminService.Domain.Entities;

/// <summary>
/// Represents a route master record
/// </summary>
public class RouteMaster : BaseEntity
{
    /// <summary>
    /// Route identifier
    /// </summary>
    public long RouteId { get; set; }

    /// <summary>
    /// Route name
    /// </summary>
    public string RouteName { get; set; } = string.Empty;

    /// <summary>
    /// Navigation property to area mappings
    /// </summary>
    public ICollection<AreaRouteMap> AreaMappings { get; set; } = new List<AreaRouteMap>();
}
