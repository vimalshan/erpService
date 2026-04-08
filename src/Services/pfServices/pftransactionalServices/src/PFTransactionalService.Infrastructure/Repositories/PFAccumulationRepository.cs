using Microsoft.EntityFrameworkCore;
using PFTransactionalService.Domain.Aggregates;
using PFTransactionalService.Domain.Interfaces;
using PFTransactionalService.Infrastructure.Persistence.EfCore;

namespace PFTransactionalService.Infrastructure.Repositories;

public class PFAccumulationRepository : IPFAccumulationRepository
{
    private readonly PFTransactionalDbContext _context;

    public PFAccumulationRepository(PFTransactionalDbContext context)
    {
        _context = context;
    }

    public async Task<PFAccumulation?> GetByIdAsync(long pfAccId, CancellationToken cancellationToken = default)
    {
        return await _context.PFAccumulations
            .Include(a => a.Contributions)
            .Include(a => a.Certificates)
            .FirstOrDefaultAsync(a => a.PfAccId == pfAccId, cancellationToken);
    }

    public async Task<PFAccumulation?> GetByEmpSysIdAsync(long empSysId, CancellationToken cancellationToken = default)
    {
        return await _context.PFAccumulations
            .Include(a => a.Contributions)
            .Include(a => a.Certificates)
            .FirstOrDefaultAsync(a => a.EmpSysId == empSysId, cancellationToken);
    }

    public async Task<IEnumerable<PFAccumulation>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PFAccumulations
            .Include(a => a.Contributions)
            .Include(a => a.Certificates)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(PFAccumulation accumulation, CancellationToken cancellationToken = default)
    {
        await _context.PFAccumulations.AddAsync(accumulation, cancellationToken);
    }

    public Task UpdateAsync(PFAccumulation accumulation, CancellationToken cancellationToken = default)
    {
        _context.PFAccumulations.Update(accumulation);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long pfAccId, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(pfAccId, cancellationToken);
        if (entity != null)
            _context.PFAccumulations.Remove(entity);
    }
}
