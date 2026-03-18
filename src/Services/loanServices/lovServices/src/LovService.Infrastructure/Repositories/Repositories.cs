using Microsoft.EntityFrameworkCore;
using LovService.Domain.Entities;
using LovService.Domain.Interfaces;
using LovService.Infrastructure.Data;

namespace LovService.Infrastructure.Repositories;

public sealed class LovTypeMastRepository(LovDbContext db) : ILovTypeMastRepository
{
    public Task<LovTypeMast?> GetByIdAsync(int lovTypeId, CancellationToken ct)
        => db.LovTypeMasts.FindAsync([lovTypeId], ct).AsTask();

    public async Task<IEnumerable<LovTypeMast>> GetAllAsync(CancellationToken ct)
        => await db.LovTypeMasts.AsNoTracking().ToListAsync(ct);

    public async Task<IEnumerable<LovTypeMast>> GetByOrgIdAsync(int orgId, CancellationToken ct)
        => await db.LovTypeMasts.AsNoTracking()
            .Where(x => x.LovOrgId == orgId)
            .ToListAsync(ct);

    public async Task AddAsync(LovTypeMast entity, CancellationToken ct)
        => await db.LovTypeMasts.AddAsync(entity, ct);

    public Task UpdateAsync(LovTypeMast entity, CancellationToken ct)
    {
        db.LovTypeMasts.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int lovTypeId, CancellationToken ct)
    {
        var entity = await db.LovTypeMasts.FindAsync([lovTypeId], ct);
        if (entity != null) db.LovTypeMasts.Remove(entity);
    }

    public Task<bool> ExistsAsync(int lovTypeId, CancellationToken ct)
        => db.LovTypeMasts.AnyAsync(x => x.LovTypeId == lovTypeId, ct);
}

public sealed class LovMasterRepository(LovDbContext db) : ILovMasterRepository
{
    public Task<LovMaster?> GetByIdAsync(long lovId, CancellationToken ct)
        => db.LovMasters.FindAsync([lovId], ct).AsTask();

    public async Task<IEnumerable<LovMaster>> GetAllAsync(CancellationToken ct)
        => await db.LovMasters.AsNoTracking().ToListAsync(ct);

    public async Task<IEnumerable<LovMaster>> GetByTypeIdAsync(int lovTypeId, CancellationToken ct)
        => await db.LovMasters.AsNoTracking()
            .Where(x => x.LovTypeId == lovTypeId)
            .ToListAsync(ct);

    public async Task AddAsync(LovMaster entity, CancellationToken ct)
        => await db.LovMasters.AddAsync(entity, ct);

    public Task UpdateAsync(LovMaster entity, CancellationToken ct)
    {
        db.LovMasters.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long lovId, CancellationToken ct)
    {
        var entity = await db.LovMasters.FindAsync([lovId], ct);
        if (entity != null) db.LovMasters.Remove(entity);
    }

    public Task<bool> ExistsAsync(long lovId, CancellationToken ct)
        => db.LovMasters.AnyAsync(x => x.LovId == lovId, ct);

    public async Task<long> GetNextIdAsync(CancellationToken ct)
    {
        var max = await db.LovMasters.AnyAsync(ct)
            ? await db.LovMasters.MaxAsync(x => x.LovId, ct)
            : 0L;
        return max + 1;
    }
}

public sealed class ProgramLovMastRepository(LovDbContext db) : IProgramLovMastRepository
{
    public Task<ProgramLovMast?> GetByIdAsync(string prlovTypeCode, string prlovCode, CancellationToken ct)
        => db.ProgramLovMasts.FindAsync([prlovCode, prlovTypeCode], ct).AsTask();

    public async Task<IEnumerable<ProgramLovMast>> GetAllAsync(CancellationToken ct)
        => await db.ProgramLovMasts.AsNoTracking().ToListAsync(ct);

    public async Task<IEnumerable<ProgramLovMast>> GetByTypeCodeAsync(string prlovTypeCode, CancellationToken ct)
        => await db.ProgramLovMasts.AsNoTracking()
            .Where(x => x.PrlovTypeCode == prlovTypeCode)
            .ToListAsync(ct);

    public async Task AddAsync(ProgramLovMast entity, CancellationToken ct)
        => await db.ProgramLovMasts.AddAsync(entity, ct);

    public Task UpdateAsync(ProgramLovMast entity, CancellationToken ct)
    {
        db.ProgramLovMasts.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(string prlovTypeCode, string prlovCode, CancellationToken ct)
    {
        var entity = await db.ProgramLovMasts.FindAsync([prlovCode, prlovTypeCode], ct);
        if (entity != null) db.ProgramLovMasts.Remove(entity);
    }

    public Task<bool> ExistsAsync(string prlovTypeCode, string prlovCode, CancellationToken ct)
        => db.ProgramLovMasts.AnyAsync(x => x.PrlovTypeCode == prlovTypeCode && x.PrlovCode == prlovCode, ct);
}
