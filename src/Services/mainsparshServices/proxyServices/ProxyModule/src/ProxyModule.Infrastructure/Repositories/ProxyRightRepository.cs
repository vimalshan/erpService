using Microsoft.EntityFrameworkCore;
using ProxyModule.Domain.Entities;
using ProxyModule.Domain.Interfaces;

namespace ProxyModule.Infrastructure.Repositories;

public class ProxyRightRepository : IProxyRightRepository
{
    private readonly Persistence.ProxyModuleDbContext _context;

    public ProxyRightRepository(Persistence.ProxyModuleDbContext context)
    {
        _context = context;
    }

    public async Task<ProxyRight?> GetByIdAsync(long proxyId, CancellationToken ct = default)
    {
        return await _context.ProxyRights.FindAsync(new object[] { proxyId }, ct);
    }

    public async Task<IEnumerable<ProxyRight>> GetByProxyUserIdAsync(long proxyUserId, CancellationToken ct = default)
    {
        return await _context.ProxyRights
            .Where(p => p.ProxyUserId == proxyUserId)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<ProxyRight>> GetByDelegatedUserIdAsync(long delegatedUserId, CancellationToken ct = default)
    {
        return await _context.ProxyRights
            .Where(p => p.DelegatedUserId == delegatedUserId)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<ProxyRight>> GetActiveProxyRightsAsync(CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        return await _context.ProxyRights
            .Where(p => p.ProxyStatus == "A" &&
                        p.ProxyStartDate <= now &&
                        (!p.ProxyEndDate.HasValue || p.ProxyEndDate.Value >= now))
            .ToListAsync(ct);
    }

    public async Task<ProxyRight> AddAsync(ProxyRight proxyRight, CancellationToken ct = default)
    {
        var entry = await _context.ProxyRights.AddAsync(proxyRight, ct);
        return entry.Entity;
    }

    public Task UpdateAsync(ProxyRight proxyRight, CancellationToken ct = default)
    {
        _context.ProxyRights.Update(proxyRight);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(long proxyId, CancellationToken ct = default)
    {
        return await _context.ProxyRights.AnyAsync(p => p.ProxyId == proxyId, ct);
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }
}
