using LovService.Application.Interfaces;
using LovService.Domain.Entities;
using LovService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LovService.Infrastructure.Repositories;

public class LovTypeRepository(LovDbContext context) : ILovTypeRepository
{
    public async Task<LovType?> GetByIdAsync(long id, CancellationToken ct = default)
        => await context.LovTypes.FindAsync([id], ct);

    public async Task<IEnumerable<LovType>> GetAllAsync(CancellationToken ct = default)
        => await context.LovTypes.AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(LovType lovType, CancellationToken ct = default)
        => await context.LovTypes.AddAsync(lovType, ct);

    public void Update(LovType lovType)
        => context.LovTypes.Update(lovType);

    public void Delete(LovType lovType)
        => context.LovTypes.Remove(lovType);

    public async Task<bool> ExistsAsync(long id, CancellationToken ct = default)
        => await context.LovTypes.AnyAsync(x => x.LovTypeId == id, ct);
}
