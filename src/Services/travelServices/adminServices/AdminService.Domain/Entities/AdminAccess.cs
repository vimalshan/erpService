namespace AdminService.Domain.Entities;

/// <summary>
/// Represents admin access configuration
/// </summary>
public class AdminAccess : BaseEntity
{
    /// <summary>
    /// Admin unit code
    /// </summary>
    public long? AdminCode { get; set; }

    /// <summary>
    /// Company code
    /// </summary>
    public string? CompanyCode { get; set; }

    /// <summary>
    /// Admin user code
    /// </summary>
    public string? LocalUserCode { get; set; }

    /// <summary>
    /// Location code
    /// </summary>
    public long? LocationCode { get; set; }

    /// <summary>
    /// Contact email
    /// </summary>
    public string? ContactEmail { get; set; }

    /// <summary>
    /// Employee system ID
    /// </summary>
    public long? EmployeeSystemId { get; set; }

    /// <summary>
    /// Navigation property to AdminUnit
    /// </summary>
    public AdminUnit? AdminUnit { get; set; }
}
