using LovService.Application.Interfaces;
using LovService.Domain.Entities;
using LovService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LovService.Infrastructure.Repositories;

public class LovMasterRepository(LovDbContext context) : ILovMasterRepository
{
    public async Task<LovMaster?> GetByIdAsync(long id, CancellationToken ct = default)
        => await context.LovMasters.FindAsync([id], ct);

    public async Task<IEnumerable<LovMaster>> GetAllAsync(CancellationToken ct = default)
        => await context.LovMasters.AsNoTracking().ToListAsync(ct);

    public async Task<IEnumerable<LovMaster>> GetByTypeIdAsync(long lovTypeId, CancellationToken ct = default)
        => await context.LovMasters.AsNoTracking()
            .Where(x => x.LovTypeId == lovTypeId)
            .ToListAsync(ct);

    public async Task AddAsync(LovMaster lovMaster, CancellationToken ct = default)
        => await context.LovMasters.AddAsync(lovMaster, ct);

    public void Update(LovMaster lovMaster)
        => context.LovMasters.Update(lovMaster);

    public void Delete(LovMaster lovMaster)
        => context.LovMasters.Remove(lovMaster);

    public async Task<bool> ExistsAsync(long id, CancellationToken ct = default)
        => await context.LovMasters.AnyAsync(x => x.LovId == id, ct);
}
