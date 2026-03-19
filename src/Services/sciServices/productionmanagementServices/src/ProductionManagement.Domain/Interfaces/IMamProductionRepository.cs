using ProductionManagement.Domain.Entities;

namespace ProductionManagement.Domain.Interfaces;

public interface IMamProductionRepository
{
    Task<IReadOnlyList<MamProductionDet>> GetProductionDetailsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MamProductionDet>> GetProductionDetailsByFgAsync(int fgCode, CancellationToken cancellationToken = default);
    Task<MamProductionDet> AddDetailAsync(MamProductionDet detail, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MamProductionMap>> GetProductionMapsAsync(CancellationToken cancellationToken = default);
    Task<MamProductionMap> AddMapAsync(MamProductionMap map, CancellationToken cancellationToken = default);
}
