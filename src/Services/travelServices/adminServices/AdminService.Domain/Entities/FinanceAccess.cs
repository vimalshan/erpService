namespace AdminService.Domain.Entities;

/// <summary>
/// Represents finance access control
/// </summary>
public class FinanceAccess : BaseEntity
{
    /// <summary>
    /// Finance identifier
    /// </summary>
    public long FinanceNo { get; set; }

    /// <summary>
    /// Finance unit identifier
    /// </summary>
    public long? UnitId { get; set; }

    /// <summary>
    /// User identifier
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// User PIN number
    /// </summary>
    public decimal? UserPin { get; set; }

    /// <summary>
    /// Finance email identifier
    /// </summary>
    public string? EmailId { get; set; }

    /// <summary>
    /// Navigation property to FinanceUnit
    /// </summary>
    public FinanceUnit? FinanceUnit { get; set; }
}
