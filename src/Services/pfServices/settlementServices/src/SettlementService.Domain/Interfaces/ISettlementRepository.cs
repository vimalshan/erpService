using SettlementService.Domain.Aggregates;

namespace SettlementService.Domain.Interfaces;

public interface ISettlementRepository
{
    Task<Settlement?> GetByIdAsync(long settlementNumber, CancellationToken cancellationToken = default);
    Task<IEnumerable<Settlement>> GetByMemberNoAsync(long memberNo, CancellationToken cancellationToken = default);
    Task<IEnumerable<Settlement>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Settlement settlement, CancellationToken cancellationToken = default);
    Task UpdateAsync(Settlement settlement, CancellationToken cancellationToken = default);
    Task DeleteAsync(long settlementNumber, CancellationToken cancellationToken = default);
}
