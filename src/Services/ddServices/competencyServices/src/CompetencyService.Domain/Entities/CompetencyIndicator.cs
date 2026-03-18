using CompetencyService.Domain.Common;
using CompetencyService.Domain.Exceptions;

namespace CompetencyService.Domain.Entities;

/// <summary>Maps to DD_COMPETENCY_IND — indicators (positive/negative) per band.</summary>
public class CompetencyIndicator : BaseEntity
{
    public decimal? SerialNo { get; private set; }      // SRL_NO
    public string? Band { get; private set; }           // BAND
    public decimal? CompetencyNo { get; private set; }  // COMP_NUM
    public char? IndicatorFlag { get; private set; }    // IND_FLAG (P/N)
    public string? IndicatorDefinition { get; private set; } // IND_DEFN

    private CompetencyIndicator() { }

    public static CompetencyIndicator Create(
        decimal? srlNo, string? band, decimal? compNum,
        char? flag, string? definition)
    {
        if (flag.HasValue && flag != 'P' && flag != 'N')
            throw new CompetencyDomainException("IndicatorFlag must be 'P' or 'N'.");

        return new CompetencyIndicator
        {
            SerialNo = srlNo,
            Band = band,
            CompetencyNo = compNum,
            IndicatorFlag = flag,
            IndicatorDefinition = definition
        };
    }
}
