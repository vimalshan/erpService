using ProxyModule.Domain.Entities;

namespace ProxyModule.Domain.Interfaces;

public interface IProxyRightRepository
{
    Task<ProxyRight?> GetByIdAsync(long proxyId, CancellationToken ct = default);
    Task<IEnumerable<ProxyRight>> GetByProxyUserIdAsync(long proxyUserId, CancellationToken ct = default);
    Task<IEnumerable<ProxyRight>> GetByDelegatedUserIdAsync(long delegatedUserId, CancellationToken ct = default);
    Task<IEnumerable<ProxyRight>> GetActiveProxyRightsAsync(CancellationToken ct = default);
    Task<ProxyRight> AddAsync(ProxyRight proxyRight, CancellationToken ct = default);
    Task UpdateAsync(ProxyRight proxyRight, CancellationToken ct = default);
    Task<bool> ExistsAsync(long proxyId, CancellationToken ct = default);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
