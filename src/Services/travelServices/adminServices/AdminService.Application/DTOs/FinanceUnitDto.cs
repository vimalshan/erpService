namespace AdminService.Application.DTOs;

/// <summary>
/// Data transfer object for FinanceUnit
/// </summary>
public class FinanceUnitDto
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public long Id { get; set; }

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
    /// Oracle code
    /// </summary>
    public long? OracleCode { get; set; }

    /// <summary>
    /// Location option
    /// </summary>
    public string? LocationOption { get; set; }

    /// <summary>
    /// Creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Modified timestamp
    /// </summary>
    public DateTime? ModifiedAt { get; set; }
}
