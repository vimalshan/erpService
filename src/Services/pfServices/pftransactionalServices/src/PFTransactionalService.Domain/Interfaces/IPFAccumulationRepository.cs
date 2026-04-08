using PFTransactionalService.Domain.Aggregates;

namespace PFTransactionalService.Domain.Interfaces;

public interface IPFAccumulationRepository
{
    Task<PFAccumulation?> GetByIdAsync(long pfAccId, CancellationToken cancellationToken = default);
    Task<PFAccumulation?> GetByEmpSysIdAsync(long empSysId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PFAccumulation>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(PFAccumulation accumulation, CancellationToken cancellationToken = default);
    Task UpdateAsync(PFAccumulation accumulation, CancellationToken cancellationToken = default);
    Task DeleteAsync(long pfAccId, CancellationToken cancellationToken = default);
}
