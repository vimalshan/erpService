using EnergyService.Domain.Entities;
using EnergyService.Domain.Interfaces;
using EnergyService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnergyService.Infrastructure.Repositories;

public class EcProcessAccessRepository : IEcProcessAccessRepository
{
    private readonly EnergyDbContext _context;

    public EcProcessAccessRepository(EnergyDbContext context) => _context = context;

    public async Task<IReadOnlyList<EcProcessAccess>> GetByProcessIdAsync(int processId, CancellationToken ct = default)
        => await _context.EcProcessAccesses
            .Where(a => a.PaProcessId == processId)
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task UpsertAsync(EcProcessAccess entity, CancellationToken ct = default)
    {
        var existing = await _context.EcProcessAccesses
            .FirstOrDefaultAsync(a => a.PaProcessId == entity.PaProcessId && a.PaEmpSysId == entity.PaEmpSysId, ct);

        if (existing is not null)
        {
            existing.PaCloseDate = entity.PaCloseDate;
            existing.PaLastModifiedBy = entity.PaLastModifiedBy;
            existing.PaLastModifiedOn = entity.PaLastModifiedOn;
            _context.EcProcessAccesses.Update(existing);
        }
        else
        {
            await _context.EcProcessAccesses.AddAsync(entity, ct);
        }
    }
}
