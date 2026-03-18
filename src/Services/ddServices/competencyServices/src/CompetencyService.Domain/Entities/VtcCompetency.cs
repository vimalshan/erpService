using CompetencyService.Domain.Common;

namespace CompetencyService.Domain.Entities;

/// <summary>Maps to DD_VTCCOMPETENCY.</summary>
public class VtcCompetency : BaseEntity
{
    public decimal? SerialNo { get; private set; }
    public string? Band { get; private set; }
    public decimal? CompetencyNo { get; private set; }
    public string? CompetencyName { get; private set; }

    private VtcCompetency() { }

    public static VtcCompetency Create(decimal? srlNo, string? band, decimal? compNum, string? compName) =>
        new() { SerialNo = srlNo, Band = band, CompetencyNo = compNum, CompetencyName = compName };
}
