using Microsoft.EntityFrameworkCore;
using TrustService.Application.Common.Interfaces;
using TrustService.Domain.Entities;
using TrustService.Infrastructure.Persistence;

namespace TrustService.Infrastructure.Repositories;

public class TrustRepository : ITrustRepository
{
    private readonly TrustDbContext _context;

    public TrustRepository(TrustDbContext context)
    {
        _context = context;
    }

    public async Task<TrustMaster?> GetByCodeAsync(string trustCode, CancellationToken cancellationToken = default)
    {
        return await _context.TrustMasters
            .Include(t => t.FundTypes)
            .Include(t => t.Roles)
            .Include(t => t.Approvers)
            .Include(t => t.Configurations)
            .Include(t => t.Units)
            .FirstOrDefaultAsync(t => t.TrustCode == trustCode, cancellationToken);
    }

    public async Task<IReadOnlyList<TrustMaster>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TrustMasters
            .Include(t => t.FundTypes)
            .Include(t => t.Roles)
            .Include(t => t.Units)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TrustMaster>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.TrustMasters
            .Include(t => t.FundTypes)
            .Include(t => t.Roles)
            .Include(t => t.Units)
            .Where(t => t.TrustStatus == "A")
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TrustMaster trust, CancellationToken cancellationToken = default)
    {
        await _context.TrustMasters.AddAsync(trust, cancellationToken);
    }

    public Task UpdateAsync(TrustMaster trust, CancellationToken cancellationToken = default)
    {
        _context.TrustMasters.Update(trust);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(string trustCode, CancellationToken cancellationToken = default)
    {
        return await _context.TrustMasters.AnyAsync(t => t.TrustCode == trustCode, cancellationToken);
    }
}
