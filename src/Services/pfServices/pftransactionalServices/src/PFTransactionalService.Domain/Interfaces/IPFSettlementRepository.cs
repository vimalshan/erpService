using PFTransactionalService.Domain.Entities;

namespace PFTransactionalService.Domain.Interfaces;

public interface IPFSettlementRepository
{
    Task<PFSettlement?> GetByIdAsync(long settlementId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PFSettlement>> GetByEmpSysIdAsync(long empSysId, CancellationToken cancellationToken = default);
    Task<IEnumerable<PFSettlement>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(PFSettlement settlement, CancellationToken cancellationToken = default);
    Task UpdateAsync(PFSettlement settlement, CancellationToken cancellationToken = default);
}
