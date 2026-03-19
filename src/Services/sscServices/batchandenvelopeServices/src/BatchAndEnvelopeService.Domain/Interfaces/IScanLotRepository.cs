using BatchAndEnvelopeService.Domain.Entities;

namespace BatchAndEnvelopeService.Domain.Interfaces;

public interface IScanLotRepository
{
    Task<ScanLotMaster?> GetByIdAsync(long lotNo, CancellationToken ct = default);
    Task<IEnumerable<ScanLotMaster>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(ScanLotMaster scanLot, CancellationToken ct = default);
    Task UpdateAsync(ScanLotMaster scanLot, CancellationToken ct = default);
}
