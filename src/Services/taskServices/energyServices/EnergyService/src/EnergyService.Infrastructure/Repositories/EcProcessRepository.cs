using EnergyService.Domain.Entities;
using EnergyService.Domain.Interfaces;
using EnergyService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnergyService.Infrastructure.Repositories;

public class EcProcessRepository : IEcProcessRepository
{
    private readonly EnergyDbContext _context;

    public EcProcessRepository(EnergyDbContext context) => _context = context;

    public async Task<EcProcess?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.EcProcesses
            .Include(p => p.ProcessAccesses)
            .Include(p => p.ProcessMailIds)
            .Include(p => p.Readings)
            .FirstOrDefaultAsync(p => p.EcProcessId == id, ct);

    public async Task<IReadOnlyList<EcProcess>> GetAllAsync(CancellationToken ct = default)
        => await _context.EcProcesses.AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(EcProcess entity, CancellationToken ct = default)
        => await _context.EcProcesses.AddAsync(entity, ct);

    public void Update(EcProcess entity)
        => _context.EcProcesses.Update(entity);

    public void Delete(EcProcess entity)
        => _context.EcProcesses.Remove(entity);
}
