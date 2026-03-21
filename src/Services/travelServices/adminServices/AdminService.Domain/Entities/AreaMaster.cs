namespace AdminService.Domain.Entities;

/// <summary>
/// Represents an area master record
/// </summary>
public class AreaMaster : BaseEntity
{
    /// <summary>
    /// Area identifier
    /// </summary>
    public long AreaId { get; set; }

    /// <summary>
    /// Area name
    /// </summary>
    public string AreaName { get; set; } = string.Empty;

    /// <summary>
    /// Navigation property to route mappings
    /// </summary>
    public ICollection<AreaRouteMap> RouteMappings { get; set; } = new List<AreaRouteMap>();
}
