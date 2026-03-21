using EnergyService.Domain.Entities;
using EnergyService.Domain.Interfaces;
using EnergyService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnergyService.Infrastructure.Repositories;

public class EcReadingRepository : IEcReadingRepository
{
    private readonly EnergyDbContext _context;

    public EcReadingRepository(EnergyDbContext context) => _context = context;

    public async Task<EcReading?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.EcReadings.FirstOrDefaultAsync(r => r.EbId == id, ct);

    public async Task<IReadOnlyList<EcReading>> GetByProcessIdAsync(int processId, CancellationToken ct = default)
        => await _context.EcReadings.Where(r => r.EbProcessId == processId)
            .OrderByDescending(r => r.EbDate)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<long?> GetLastReadingValueAsync(string unitCode, int processId, CancellationToken ct = default)
        => await _context.EcReadings
            .Where(r => r.EbUnitCode == unitCode && r.EbProcessId == processId)
            .OrderByDescending(r => r.EbDate)
            .Select(r => r.EbReading)
            .FirstOrDefaultAsync(ct);

    public async Task AddAsync(EcReading entity, CancellationToken ct = default)
        => await _context.EcReadings.AddAsync(entity, ct);
}
