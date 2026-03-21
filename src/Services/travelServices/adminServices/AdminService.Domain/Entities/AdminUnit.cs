namespace AdminService.Domain.Entities;

/// <summary>
/// Represents a travel administration unit
/// </summary>
public class AdminUnit : BaseEntity
{
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
    /// Image URL for admin unit
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Sort order
    /// </summary>
    public long? SortOrder { get; set; }

    /// <summary>
    /// List of access configurations
    /// </summary>
    public ICollection<AdminAccess> AccessConfigurations { get; set; } = new List<AdminAccess>();

    /// <summary>
    /// List of contact details
    /// </summary>
    public ICollection<AdminContact> ContactDetails { get; set; } = new List<AdminContact>();
}
