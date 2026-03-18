using Document.Domain.Common;

namespace Document.Domain.Entities;

/// <summary>
/// Maps to DD_APPRAISALLETTER — master appraisal letter template.
/// </summary>
public class AppraisalLetter : BaseEntity
{
    public decimal SerialNo { get; private set; }
    public decimal? BandCode { get; private set; }
    public string? LetterType { get; private set; }       // APR, AN1, AN2
    public DateTime? FromDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string? Paragraph1 { get; private set; }
    public string? Paragraph2 { get; private set; }
    public string? Paragraph3 { get; private set; }
    public string? Paragraph4 { get; private set; }
    public string? Paragraph5 { get; private set; }
    public DateTime? EffectiveDate { get; private set; }
    public DateTime? BasicPayEffectiveDate { get; private set; }
    public DateTime? PrintDate { get; private set; }

    private AppraisalLetter() { }

    public static AppraisalLetter Create(
        decimal serialNo,
        string? letterType,
        DateTime? fromDate,
        DateTime? endDate,
        string? paragraph1 = null,
        string? paragraph2 = null,
        DateTime? effectiveDate = null)
    {
        return new AppraisalLetter
        {
            SerialNo = serialNo,
            LetterType = letterType,
            FromDate = fromDate,
            EndDate = endDate,
            Paragraph1 = paragraph1,
            Paragraph2 = paragraph2,
            EffectiveDate = effectiveDate
        };
    }
}
