using Microsoft.EntityFrameworkCore;
using CompetencyService.Domain.Entities;
using CompetencyService.Domain.Interfaces;

namespace CompetencyService.Infrastructure.Persistence.Repositories;

public class CompetencyRepository(CompetencyDbContext context) : ICompetencyRepository
{
    public async Task<CompetencyMaster?> GetByIdAsync(decimal id, CancellationToken ct) =>
        await context.CompetencyMasters.FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<IEnumerable<CompetencyMaster>> GetAllAsync(CancellationToken ct) =>
        await context.CompetencyMasters.ToListAsync(ct);

    public async Task<IEnumerable<CompetencyMaster>> GetByTypeAsync(string type, CancellationToken ct) =>
        await context.CompetencyMasters
            .Where(c => c.CompetencyType == type)
            .ToListAsync(ct);

    public async Task AddAsync(CompetencyMaster entity, CancellationToken ct) =>
        await context.CompetencyMasters.AddAsync(entity, ct);

    public void Update(CompetencyMaster entity) =>
        context.CompetencyMasters.Update(entity);

    public void Delete(CompetencyMaster entity) =>
        context.CompetencyMasters.Remove(entity);
}

public class EmpSpecificCompetencyRepository(CompetencyDbContext context) : IEmpSpecificCompetencyRepository
{
    public async Task<IEnumerable<EmpSpecificCompetency>> GetByEmpAsync(decimal empSysId, decimal yearId, CancellationToken ct) =>
        await context.EmpSpecificCompetencies
            .Where(e => e.EmpSysId == empSysId && e.YearId == yearId)
            .ToListAsync(ct);

    public async Task AddAsync(EmpSpecificCompetency entity, CancellationToken ct) =>
        await context.EmpSpecificCompetencies.AddAsync(entity, ct);

    public void Delete(EmpSpecificCompetency entity) =>
        context.EmpSpecificCompetencies.Remove(entity);
}

public class RoleSpecificRepository(CompetencyDbContext context) : IRoleSpecificRepository
{
    public async Task<IEnumerable<RoleSpecific>> GetByEmpAsync(decimal empSysId, CancellationToken ct) =>
        await context.RoleSpecifics
            .Where(r => r.EmpSysId == empSysId)
            .ToListAsync(ct);

    public async Task AddAsync(RoleSpecific entity, CancellationToken ct) =>
        await context.RoleSpecifics.AddAsync(entity, ct);

    public void Update(RoleSpecific entity) =>
        context.RoleSpecifics.Update(entity);
}

public class CompetencyRatingScaleRepository(CompetencyDbContext context) : ICompetencyRatingScaleRepository
{
    public async Task<CompetencyRatingScale?> GetByCompetencyIdAsync(decimal competencyId, CancellationToken ct) =>
        await context.CompetencyRatingScales.FirstOrDefaultAsync(r => r.CompetencyId == competencyId, ct);

    public async Task AddAsync(CompetencyRatingScale entity, CancellationToken ct) =>
        await context.CompetencyRatingScales.AddAsync(entity, ct);

    public void Update(CompetencyRatingScale entity) =>
        context.CompetencyRatingScales.Update(entity);
}
