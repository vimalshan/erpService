namespace AdminService.Application.DTOs;

/// <summary>
/// Data transfer object for AdminUnit
/// </summary>
public class AdminUnitDto
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Admin unit code
    /// </summary>
    public long AdminCode { get; set; }

    /// <summary>
    /// Admin unit name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Admin type (T=Travel, S=Stay, M=Meeting)
    /// </summary>
    public string? AdminType { get; set; }

    /// <summary>
    /// Unit code
    /// </summary>
    public string? UnitCode { get; set; }

    /// <summary>
    /// Cab unit identifier
    /// </summary>
    public long? CabUnit { get; set; }

    /// <summary>
    /// Image URL
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Sort order
    /// </summary>
    public long? SortOrder { get; set; }

    /// <summary>
    /// Creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Modified timestamp
    /// </summary>
    public DateTime? ModifiedAt { get; set; }
}
