using TrustService.Domain.Entities;

namespace TrustService.Application.Common.Interfaces;

public interface ITrustRepository
{
    Task<TrustMaster?> GetByCodeAsync(string trustCode, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TrustMaster>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TrustMaster>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task AddAsync(TrustMaster trust, CancellationToken cancellationToken = default);
    Task UpdateAsync(TrustMaster trust, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string trustCode, CancellationToken cancellationToken = default);
}
