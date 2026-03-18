using Dapper;
using DispatchPlanning.Domain.Aggregates;
using DispatchPlanning.Domain.Entities;
using DispatchPlanning.Domain.Interfaces;
using DispatchPlanning.Domain.ValueObjects;
using DispatchPlanning.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace DispatchPlanning.Infrastructure.Repositories;

public class DispatchPlanRepository : IDispatchPlanRepository
{
    private readonly DispatchPlanningDbContext _context;

    public DispatchPlanRepository(DispatchPlanningDbContext context) => _context = context;

    public async Task<DispatchPlanAggregate?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.DispatchPlanHeaders
            .Include(h => h.Items)
            .Include(h => h.SubGroupTargets)
            .FirstOrDefaultAsync(h => h.DispatchPlanHeaderId == id, ct);

    public async Task<IEnumerable<DispatchPlanAggregate>> GetAllAsync(int companyUnitId, CancellationToken ct = default)
        => await _context.DispatchPlanHeaders
            .Where(h => h.CompanyUnitId == companyUnitId)
            .OrderByDescending(h => h.PlanMonth)
            .ToListAsync(ct);

    public async Task<int> AddAsync(DispatchPlanAggregate plan, CancellationToken ct = default)
    {
        _context.DispatchPlanHeaders.Add(plan);
        await _context.SaveChangesAsync(ct);
        return plan.DispatchPlanHeaderId;
    }

    public async Task UpdateAsync(DispatchPlanAggregate plan, CancellationToken ct = default)
    {
        _context.DispatchPlanHeaders.Update(plan);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var plan = await _context.DispatchPlanHeaders.FindAsync(new object[] { id }, ct);
        if (plan is not null)
        {
            _context.DispatchPlanHeaders.Remove(plan);
            await _context.SaveChangesAsync(ct);
        }
    }
}

public class DispatchPlanMainGroupRepository : IDispatchPlanMainGroupRepository
{
    private readonly DispatchPlanningDbContext _context;

    public DispatchPlanMainGroupRepository(DispatchPlanningDbContext context) => _context = context;

    public async Task<DispatchPlanMainGroup?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.DispatchPlanMainGroups.FindAsync(new object[] { id }, ct);

    public async Task<IEnumerable<DispatchPlanMainGroup>> GetAllAsync(int companyUnitId, CancellationToken ct = default)
        => await _context.DispatchPlanMainGroups
            .Where(g => g.CompanyUnitId == companyUnitId)
            .OrderBy(g => g.MgDisplayOrder)
            .ToListAsync(ct);

    public async Task AddAsync(DispatchPlanMainGroup group, CancellationToken ct = default)
    {
        _context.DispatchPlanMainGroups.Add(group);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(DispatchPlanMainGroup group, CancellationToken ct = default)
    {
        _context.DispatchPlanMainGroups.Update(group);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var group = await _context.DispatchPlanMainGroups.FindAsync(new object[] { id }, ct);
        if (group is not null)
        {
            _context.DispatchPlanMainGroups.Remove(group);
            await _context.SaveChangesAsync(ct);
        }
    }
}

public class DispatchPlanSubGroupRepository : IDispatchPlanSubGroupRepository
{
    private readonly DispatchPlanningDbContext _context;

    public DispatchPlanSubGroupRepository(DispatchPlanningDbContext context) => _context = context;

    public async Task<DispatchPlanSubGroup?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.DispatchPlanSubGroups.FindAsync(new object[] { id }, ct);

    public async Task<IEnumerable<DispatchPlanSubGroup>> GetByMainGroupAsync(int mainGroupId, CancellationToken ct = default)
        => await _context.DispatchPlanSubGroups
            .Where(sg => sg.MainGroupId == mainGroupId)
            .OrderBy(sg => sg.SgDisplayOrder)
            .ToListAsync(ct);

    public async Task AddAsync(DispatchPlanSubGroup subGroup, CancellationToken ct = default)
    {
        _context.DispatchPlanSubGroups.Add(subGroup);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(DispatchPlanSubGroup subGroup, CancellationToken ct = default)
    {
        _context.DispatchPlanSubGroups.Update(subGroup);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var sg = await _context.DispatchPlanSubGroups.FindAsync(new object[] { id }, ct);
        if (sg is not null)
        {
            _context.DispatchPlanSubGroups.Remove(sg);
            await _context.SaveChangesAsync(ct);
        }
    }
}

public class DispatchPlanBreakupItemRepository : IDispatchPlanBreakupItemRepository
{
    private readonly DispatchPlanningDbContext _context;
    private readonly string _connectionString;

    public DispatchPlanBreakupItemRepository(DispatchPlanningDbContext context,
        Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
        _context = context;
        _connectionString = configuration["ConnectionStrings:SCIDB"]!;
    }

    public async Task<DispatchPlanBreakupItem?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _context.DispatchPlanBreakupItems.FindAsync(new object[] { id }, ct);

    public async Task<IEnumerable<DispatchPlanBreakupItem>> GetBySubGroupAsync(int subGroupId, CancellationToken ct = default)
    {
        // Using Dapper for read-heavy query
        using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<DispatchPlanBreakupItem>(
            @"SELECT BREAKUP_ITEM_ID AS BreakupItemId, SUB_GROUP_ID AS SubGroupId,
                     PRODUCT_ID AS ProductId, BREAKUP_ITEM_DESC AS BreakupItemDesc,
                     UNIT_ID AS UnitId, MAIN_PRODUCT_UNITS_CONFACTOR AS MainProductUnitsConFactor,
                     BI_DISPLAY_ORDER AS BiDisplayOrder, EFFECTIVE_DATE AS EffectiveDate,
                     CLOSURE_DATE AS ClosureDate, SCI_USER_ID_CREATED AS SciUserIdCreated,
                     CREATION_DATE AS CreationDate, PACKAGE_ID AS PackageId
              FROM DISPATCH_PLAN_BREAKUP_ITEM
              WHERE SUB_GROUP_ID = @SubGroupId
              ORDER BY BI_DISPLAY_ORDER",
            new { SubGroupId = subGroupId });
    }

    public async Task AddAsync(DispatchPlanBreakupItem item, CancellationToken ct = default)
    {
        _context.DispatchPlanBreakupItems.Add(item);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(DispatchPlanBreakupItem item, CancellationToken ct = default)
    {
        _context.DispatchPlanBreakupItems.Update(item);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, int deletedBy, CancellationToken ct = default)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(
            @"INSERT INTO LOG_DISPATCH_PLAN_BREAKUP_ITEM
                (BREAKUP_ITEM_ID, SUB_GROUP_ID, PRODUCT_ID, BREAKUP_ITEM_DESC, UNIT_ID,
                 MAIN_PRODUCT_UNITS_CONFACTOR, BI_DISPLAY_ORDER, EFFECTIVE_DATE, CLOSURE_DATE,
                 SCI_USER_ID_CREATED, CREATION_DATE, SCI_USER_ID_MODIFIED, MODIFIED_DATE,
                 SCI_USER_ID_DELETE, DELETE_DATE, PACKAGE_ID)
              SELECT BREAKUP_ITEM_ID, SUB_GROUP_ID, PRODUCT_ID, BREAKUP_ITEM_DESC, UNIT_ID,
                     MAIN_PRODUCT_UNITS_CONFACTOR, BI_DISPLAY_ORDER, EFFECTIVE_DATE, ISNULL(CLOSURE_DATE,''),
                     SCI_USER_ID_CREATED, CREATION_DATE, SCI_USER_ID_MODIFIED, MODIFIED_DATE,
                     @DeletedBy, GETDATE(), PACKAGE_ID
              FROM DISPATCH_PLAN_BREAKUP_ITEM WHERE BREAKUP_ITEM_ID = @Id;
              DELETE FROM DISPATCH_PLAN_BREAKUP_ITEM WHERE BREAKUP_ITEM_ID = @Id;",
            new { Id = id, DeletedBy = deletedBy });
    }
}
