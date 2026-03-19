using Microsoft.EntityFrameworkCore;
using ProductionManagement.Domain.Entities;
using ProductionManagement.Domain.Interfaces;
using ProductionManagement.Infrastructure.Persistence;

namespace ProductionManagement.Infrastructure.Repositories;

public class NormsRepository : INormsRepository
{
    private readonly ProductionManagementDbContext _context;

    public NormsRepository(ProductionManagementDbContext context) => _context = context;

    public async Task<NormsMain?> GetByIdAsync(long normNo, CancellationToken cancellationToken = default)
    {
        return await _context.NormsMain
            .Include(n => n.NormsMasters)
            .FirstOrDefaultAsync(n => n.NormNo == normNo, cancellationToken);
    }

    public async Task<IReadOnlyList<NormsMain>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.NormsMain
            .Include(n => n.NormsMasters)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<NormsMain> AddAsync(NormsMain norm, CancellationToken cancellationToken = default)
    {
        await _context.NormsMain.AddAsync(norm, cancellationToken);
        return norm;
    }

    public Task UpdateAsync(NormsMain norm, CancellationToken cancellationToken = default)
    {
        _context.NormsMain.Update(norm);
        return Task.CompletedTask;
    }

    public async Task<IReadOnlyList<NormsMaster>> GetNormsMastersByNormNoAsync(long normNo, CancellationToken cancellationToken = default)
    {
        return await _context.NormsMasters
            .Where(nm => nm.NormNo == normNo)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
