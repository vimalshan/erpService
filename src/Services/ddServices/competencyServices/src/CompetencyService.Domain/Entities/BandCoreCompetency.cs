using CompetencyService.Domain.Common;

namespace CompetencyService.Domain.Entities;

/// <summary>Maps to BAND_CORECOMPETENCY — links a band to core competencies.</summary>
public class BandCoreCompetency : BaseEntity
{
    public decimal BandId { get; private set; }
    public decimal CompetencyId { get; private set; }

    private BandCoreCompetency() { }

    public static BandCoreCompetency Create(decimal bandId, decimal competencyId) =>
        new() { BandId = bandId, CompetencyId = competencyId };
}
