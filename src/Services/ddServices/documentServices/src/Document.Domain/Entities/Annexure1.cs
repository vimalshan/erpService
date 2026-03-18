using Document.Domain.Common;

namespace Document.Domain.Entities;

/// <summary>
/// Maps to DD_ANNEXURE1 — annexure 1 attachment for appraisal letters.
/// </summary>
public class Annexure1 : BaseEntity
{
    public decimal? CreatedByPin { get; private set; }
    public decimal? UserPin { get; private set; }
    public string? UserName { get; private set; }
    public string? Answer1 { get; private set; }
    public string? Answer2 { get; private set; }
    public string? Answer3 { get; private set; }
    public string? Answer4 { get; private set; }
    public string? SignatoryName { get; private set; }
    public string? SignatoryDesignation { get; private set; }
    public string? UserRandomNumber { get; private set; }
    public string? UserUnitCode { get; private set; }
    public DateTime? PrintDate { get; private set; }
    public decimal? AppraisalLumpsum { get; private set; }
    public decimal? AppraisalBasicPay { get; private set; }
    public decimal? AppraisalFlexiPay { get; private set; }
    public DateTime? EffectiveDate { get; private set; }

    private Annexure1() { }

    public static Annexure1 Create(decimal? createdByPin, decimal? userPin, string? userName)
        => new() { CreatedByPin = createdByPin, UserPin = userPin, UserName = userName };
}
