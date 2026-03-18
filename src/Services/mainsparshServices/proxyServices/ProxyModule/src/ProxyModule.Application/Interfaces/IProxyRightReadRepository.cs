using ProxyModule.Application.DTOs;

namespace ProxyModule.Application.Interfaces;

public interface IProxyRightReadRepository
{
    Task<ProxyRightDto?> GetByIdAsync(long proxyId, CancellationToken ct = default);
    Task<IEnumerable<ProxyRightDto>> GetByProxyUserIdAsync(long proxyUserId, CancellationToken ct = default);
    Task<IEnumerable<ProxyRightDto>> GetByDelegatedUserIdAsync(long delegatedUserId, CancellationToken ct = default);
    Task<IEnumerable<ProxyRightDto>> GetActiveProxyRightsAsync(CancellationToken ct = default);
}
