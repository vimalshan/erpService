using EnergyService.Domain.Entities;
using EnergyService.Domain.Interfaces;
using EnergyService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnergyService.Infrastructure.Repositories;

public class EcProcessMailIdRepository : IEcProcessMailIdRepository
{
    private readonly EnergyDbContext _context;

    public EcProcessMailIdRepository(EnergyDbContext context) => _context = context;

    public async Task<IReadOnlyList<EcProcessMailId>> GetByProcessIdAsync(int processId, CancellationToken ct = default)
        => await _context.EcProcessMailIds
            .Where(m => m.PmProcessId == processId)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task AddAsync(EcProcessMailId entity, CancellationToken ct = default)
        => await _context.EcProcessMailIds.AddAsync(entity, ct);

    public void Update(EcProcessMailId entity)
        => _context.EcProcessMailIds.Update(entity);
}
