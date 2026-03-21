namespace AdminService.Domain.Entities;

/// <summary>
/// Represents a finance unit
/// </summary>
public class FinanceUnit : BaseEntity
{
    /// <summary>
    /// Unit identifier
    /// </summary>
    public long UnitId { get; set; }

    /// <summary>
    /// Unit code
    /// </summary>
    public string? UnitCode { get; set; }

    /// <summary>
    /// Unit name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Oracle code for the unit
    /// </summary>
    public long? OracleCode { get; set; }

    /// <summary>
    /// Location option/segment code
    /// </summary>
    public string? LocationOption { get; set; }

    /// <summary>
    /// List of finance access configurations
    /// </summary>
    public ICollection<FinanceAccess> AccessConfigurations { get; set; } = new List<FinanceAccess>();
}
