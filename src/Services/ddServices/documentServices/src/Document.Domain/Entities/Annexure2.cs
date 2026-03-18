using Document.Domain.Common;

namespace Document.Domain.Entities;

/// <summary>
/// Maps to DD_ANNEXURE2 — annexure 2 attachment (pay revision details).
/// </summary>
public class Annexure2 : BaseEntity
{
    public decimal? CreatedByPin { get; private set; }
    public decimal? UserPin { get; private set; }
    public string? UserName { get; private set; }
    public decimal? BasicOld { get; private set; }
    public decimal? BasicNew { get; private set; }
    public decimal? FlexiPay { get; private set; }
    public string? SignatoryName { get; private set; }
    public string? SignatoryDesignation { get; private set; }
    public DateTime? EffectiveDate { get; private set; }
    public string? PrintDate { get; private set; }
    public string? BandName { get; private set; }

    private Annexure2() { }

    public static Annexure2 Create(decimal? createdByPin, decimal? userPin, string? userName)
        => new() { CreatedByPin = createdByPin, UserPin = userPin, UserName = userName };
}
