using Microsoft.EntityFrameworkCore;
using EmployeeRelations.Domain.Aggregates;
using EmployeeRelations.Domain.Interfaces;
using EmployeeRelations.Infrastructure.Persistence.EfCore;

namespace EmployeeRelations.Infrastructure.Repositories;

public class DisciplinaryRepository : IDisciplinaryRepository
{
    private readonly EmployeeRelationsDbContext _ctx;
    public DisciplinaryRepository(EmployeeRelationsDbContext ctx) => _ctx = ctx;

    public async Task<DisciplinaryMain?> GetByIdAsync(long id, CancellationToken ct) =>
        await _ctx.DisciplinaryMains
            .Include(d => d.Employees)
            .Include(d => d.Actions)
            .FirstOrDefaultAsync(d => d.Id == id, ct);

    public async Task<IEnumerable<DisciplinaryMain>> GetAllAsync(CancellationToken ct) =>
        await _ctx.DisciplinaryMains
            .Include(d => d.Employees)
            .Include(d => d.Actions)
            .ToListAsync(ct);

    public async Task AddAsync(DisciplinaryMain entity, CancellationToken ct) =>
        await _ctx.DisciplinaryMains.AddAsync(entity, ct);

    public Task UpdateAsync(DisciplinaryMain entity, CancellationToken ct)
    {
        _ctx.DisciplinaryMains.Update(entity);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long id, CancellationToken ct)
    {
        var entity = await GetByIdAsync(id, ct);
        if (entity is not null) _ctx.DisciplinaryMains.Remove(entity);
    }

    public Task<bool> ExistsAsync(long id, CancellationToken ct) =>
        _ctx.DisciplinaryMains.AnyAsync(d => d.Id == id, ct);
}

public class EwsRepository : IEwsRepository
{
    private readonly EmployeeRelationsDbContext _ctx;
    public EwsRepository(EmployeeRelationsDbContext ctx) => _ctx = ctx;

    public async Task<EwsMain?> GetByIdAsync(long id, CancellationToken ct) =>
        await _ctx.EwsMains.Include(e => e.AppInputs).FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task<IEnumerable<EwsMain>> GetByEmpAsync(long empSysId, CancellationToken ct) =>
        await _ctx.EwsMains.Include(e => e.AppInputs).Where(e => e.EmpSysId == empSysId).ToListAsync(ct);

    public async Task<IEnumerable<EwsMain>> GetByPeriodAsync(int periodNo, CancellationToken ct) =>
        await _ctx.EwsMains.Include(e => e.AppInputs).Where(e => e.PeriodNo == periodNo).ToListAsync(ct);

    public async Task AddAsync(EwsMain entity, CancellationToken ct) =>
        await _ctx.EwsMains.AddAsync(entity, ct);

    public Task UpdateAsync(EwsMain entity, CancellationToken ct)
    {
        _ctx.EwsMains.Update(entity);
        return Task.CompletedTask;
    }
}

public class SurveyRepository : ISurveyRepository
{
    private readonly EmployeeRelationsDbContext _ctx;
    public SurveyRepository(EmployeeRelationsDbContext ctx) => _ctx = ctx;

    public async Task<SurveyMaster?> GetByIdAsync(long id, CancellationToken ct) =>
        await _ctx.SurveyMasters
            .Include(s => s.Questions).ThenInclude(q => q.Options)
            .Include(s => s.Responses).ThenInclude(r => r.Details)
            .FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<IEnumerable<SurveyMaster>> GetAllAsync(CancellationToken ct) =>
        await _ctx.SurveyMasters
            .Include(s => s.Questions).ThenInclude(q => q.Options)
            .ToListAsync(ct);

    public async Task AddAsync(SurveyMaster entity, CancellationToken ct) =>
        await _ctx.SurveyMasters.AddAsync(entity, ct);

    public Task UpdateAsync(SurveyMaster entity, CancellationToken ct)
    {
        _ctx.SurveyMasters.Update(entity);
        return Task.CompletedTask;
    }

    public async Task<SurveyResponseMain?> GetResponseAsync(long responseId, CancellationToken ct) =>
        await _ctx.SurveyResponseMains.Include(r => r.Details).FirstOrDefaultAsync(r => r.ResponseId == responseId, ct);

    public async Task AddResponseAsync(SurveyResponseMain entity, CancellationToken ct) =>
        await _ctx.SurveyResponseMains.AddAsync(entity, ct);
}

public class UnitOfWork : IUnitOfWork
{
    private readonly EmployeeRelationsDbContext _ctx;
    public UnitOfWork(EmployeeRelationsDbContext ctx) => _ctx = ctx;
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _ctx.SaveChangesAsync(cancellationToken);
}
