using DevelopmentService.Domain.Entities;

namespace DevelopmentService.Domain.Interfaces;

public interface ICompetencyRepository
{
    Task<IEnumerable<CompetencyInd>> GetIndicatorsAsync(long? compNum, string? band, CancellationToken ct = default);
}
