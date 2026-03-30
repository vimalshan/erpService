using Microsoft.EntityFrameworkCore;
using OrganizationStructureService.Domain.Entities;
using OrganizationStructureService.Domain.Interfaces;
using OrganizationStructureService.Infrastructure.Persistence;

namespace OrganizationStructureService.Infrastructure.Repositories;

public class BusinessRepository : IBusinessRepository
{
    private readonly OrganizationDbContext _ctx;
    public BusinessRepository(OrganizationDbContext ctx) => _ctx = ctx;

    public async Task<Business?> GetByIdAsync(decimal businessId, CancellationToken ct = default) =>
        await _ctx.Businesses.FirstOrDefaultAsync(b => b.BusinessId == businessId, ct);

    public async Task<IReadOnlyList<Business>> GetAllAsync(CancellationToken ct = default) =>
        await _ctx.Businesses.ToListAsync(ct);

    public async Task<IReadOnlyList<Business>> GetActiveAsync(CancellationToken ct = default)
    {
        var all = await _ctx.Businesses.ToListAsync(ct);
        return all.Where(b => b.LiveFlag.Value == "Y").ToList();
    }

    public async Task AddAsync(Business business, CancellationToken ct = default)
    {
        await _ctx.Businesses.AddAsync(business, ct);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Business business, CancellationToken ct = default)
    {
        _ctx.Businesses.Update(business);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(decimal businessId, CancellationToken ct = default) =>
        await _ctx.Businesses.AnyAsync(b => b.BusinessId == businessId, ct);
}

public class UnitRepository : IUnitRepository
{
    private readonly OrganizationDbContext _ctx;
    public UnitRepository(OrganizationDbContext ctx) => _ctx = ctx;

    public async Task<Unit?> GetByIdAsync(decimal unitId, CancellationToken ct = default) =>
        await _ctx.Units.FirstOrDefaultAsync(u => u.UnitId == unitId, ct);

    public async Task<IReadOnlyList<Unit>> GetAllAsync(CancellationToken ct = default) =>
        await _ctx.Units.ToListAsync(ct);

    public async Task<IReadOnlyList<Unit>> GetByBusinessIdAsync(decimal businessId, CancellationToken ct = default) =>
        await _ctx.Units.Where(u => u.UnitBusinessId == businessId).ToListAsync(ct);

    public async Task<IReadOnlyList<Unit>> GetActiveAsync(CancellationToken ct = default)
    {
        var all = await _ctx.Units.ToListAsync(ct);
        return all.Where(u => u.LiveFlag.Value == "Y").ToList();
    }

    public async Task AddAsync(Unit unit, CancellationToken ct = default)
    {
        await _ctx.Units.AddAsync(unit, ct);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Unit unit, CancellationToken ct = default)
    {
        _ctx.Units.Update(unit);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsAsync(decimal unitId, CancellationToken ct = default) =>
        await _ctx.Units.AnyAsync(u => u.UnitId == unitId, ct);
}

public class DepartmentRepository : IDepartmentRepository
{
    private readonly OrganizationDbContext _ctx;
    public DepartmentRepository(OrganizationDbContext ctx) => _ctx = ctx;

    public async Task<Department?> GetByIdAsync(decimal departmentId, CancellationToken ct = default) =>
        await _ctx.Departments.FirstOrDefaultAsync(d => d.DepartmentId == departmentId, ct);

    public async Task<IReadOnlyList<Department>> GetAllAsync(CancellationToken ct = default) =>
        await _ctx.Departments.ToListAsync(ct);

    public async Task AddAsync(Department department, CancellationToken ct = default)
    {
        await _ctx.Departments.AddAsync(department, ct);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Department department, CancellationToken ct = default)
    {
        _ctx.Departments.Update(department);
        await _ctx.SaveChangesAsync(ct);
    }
}

public class DivisionRepository : IDivisionRepository
{
    private readonly OrganizationDbContext _ctx;
    public DivisionRepository(OrganizationDbContext ctx) => _ctx = ctx;

    public async Task<Division?> GetByIdAsync(decimal divisionId, CancellationToken ct = default) =>
        await _ctx.Divisions.FirstOrDefaultAsync(d => d.DivisionId == divisionId, ct);

    public async Task<IReadOnlyList<Division>> GetAllAsync(CancellationToken ct = default) =>
        await _ctx.Divisions.ToListAsync(ct);

    public async Task AddAsync(Division division, CancellationToken ct = default)
    {
        await _ctx.Divisions.AddAsync(division, ct);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Division division, CancellationToken ct = default)
    {
        _ctx.Divisions.Update(division);
        await _ctx.SaveChangesAsync(ct);
    }
}

public class GradeRepository : IGradeRepository
{
    private readonly OrganizationDbContext _ctx;
    public GradeRepository(OrganizationDbContext ctx) => _ctx = ctx;

    public async Task<Grade?> GetByIdAsync(decimal gradeId, CancellationToken ct = default) =>
        await _ctx.Grades.FirstOrDefaultAsync(g => g.GradeId == gradeId, ct);

    public async Task<IReadOnlyList<Grade>> GetAllAsync(CancellationToken ct = default) =>
        await _ctx.Grades.ToListAsync(ct);

    public async Task<IReadOnlyList<Grade>> GetActiveAsync(CancellationToken ct = default)
    {
        var all = await _ctx.Grades.ToListAsync(ct);
        return all.Where(g => g.LiveFlag?.Value == "Y").ToList();
    }

    public async Task AddAsync(Grade grade, CancellationToken ct = default)
    {
        await _ctx.Grades.AddAsync(grade, ct);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Grade grade, CancellationToken ct = default)
    {
        _ctx.Grades.Update(grade);
        await _ctx.SaveChangesAsync(ct);
    }
}

public class PositionRepository : IPositionRepository
{
    private readonly OrganizationDbContext _ctx;
    public PositionRepository(OrganizationDbContext ctx) => _ctx = ctx;

    public async Task<Position?> GetByIdAsync(decimal positionId, CancellationToken ct = default) =>
        await _ctx.Positions.FirstOrDefaultAsync(p => p.PositionId == positionId, ct);

    public async Task<IReadOnlyList<Position>> GetAllAsync(CancellationToken ct = default) =>
        await _ctx.Positions.Where(p => p.DeletedFlag == "N").ToListAsync(ct);

    public async Task<IReadOnlyList<Position>> GetByUnitCodeAsync(string unitCode, CancellationToken ct = default) =>
        await _ctx.Positions.Where(p => p.PosUnitCode == unitCode && p.DeletedFlag == "N").ToListAsync(ct);

    public async Task AddAsync(Position position, CancellationToken ct = default)
    {
        await _ctx.Positions.AddAsync(position, ct);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Position position, CancellationToken ct = default)
    {
        _ctx.Positions.Update(position);
        await _ctx.SaveChangesAsync(ct);
    }
}

public class SiteRepository : ISiteRepository
{
    private readonly OrganizationDbContext _ctx;
    public SiteRepository(OrganizationDbContext ctx) => _ctx = ctx;

    public async Task<Site?> GetByIdAsync(decimal siteId, CancellationToken ct = default) =>
        await _ctx.Sites.FirstOrDefaultAsync(s => s.SiteId == siteId, ct);

    public async Task<IReadOnlyList<Site>> GetAllAsync(CancellationToken ct = default) =>
        await _ctx.Sites.ToListAsync(ct);

    public async Task AddAsync(Site site, CancellationToken ct = default)
    {
        await _ctx.Sites.AddAsync(site, ct);
        await _ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Site site, CancellationToken ct = default)
    {
        _ctx.Sites.Update(site);
        await _ctx.SaveChangesAsync(ct);
    }
}
