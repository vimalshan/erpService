using Document.Domain.Common;

namespace Document.Domain.Entities;

/// <summary>
/// Maps to DD_APPRAISALLETTER_NEW — extended appraisal letter template.
/// </summary>
public class AppraisalLetterNew : BaseEntity
{
    public decimal SerialNo { get; private set; }
    public decimal? BandCode { get; private set; }
    public string? LetterType { get; private set; }
    public DateTime? FromDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string? Paragraph1 { get; private set; }
    public string? Paragraph2 { get; private set; }
    public string? Paragraph3 { get; private set; }
    public string? Paragraph4 { get; private set; }
    public string? Paragraph5 { get; private set; }
    public string? Paragraph6 { get; private set; }
    public DateTime? EffectiveDate { get; private set; }
    public DateTime? BasicPayEffectiveDate { get; private set; }
    public DateTime? PrintDate { get; private set; }
    public string? LetterTypeCode { get; private set; }

    private AppraisalLetterNew() { }

    public static AppraisalLetterNew Create(decimal serialNo, string? letterType)
        => new() { SerialNo = serialNo, LetterType = letterType };
}
