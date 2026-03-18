namespace ReportingService.Domain.Entities;

/// <summary>
/// DD Rating Entity
/// </summary>
public class DDRating : BaseEntity
{
    public string? UserId { get; set; }
    public long? UserPinNumber { get; set; }
    public string? BusinessCode { get; set; }
    public string? BusinessName { get; set; }
    public string? UnitCode { get; set; }
    public string? UnitName { get; set; }
    public decimal? Rating1 { get; set; }
    public decimal? Rating2 { get; set; }
    public decimal? Rating3 { get; set; }
    public decimal? Rating4 { get; set; }
    public decimal? Rating5 { get; set; }
    public decimal? TotalRating { get; set; }
    public decimal? Rating1Percentage { get; set; }
    public decimal? Rating2Percentage { get; set; }
    public decimal? Rating4Percentage { get; set; }
    public decimal? Rating3Percentage { get; set; }
    public decimal? Rating5Percentage { get; set; }
    public string? UniversitCode { get; set; }
    public string? BusinessUnitCode { get; set; }
    public decimal? TotalPercentage { get; set; }

    public DDRating() { }

    public DDRating(string? userId, string? businessCode, string? unitCode)
    {
        UserId = userId;
        BusinessCode = businessCode;
        UnitCode = unitCode;
    }
}
