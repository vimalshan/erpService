namespace TransactionService.Infrastructure.Repositories;

using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Interfaces;
using TransactionService.Infrastructure.Persistence;

public sealed class BudgetRepository : IBudgetRepository
{
    private readonly TransactionDbContext _context;
    private readonly string _connectionString;

    public BudgetRepository(TransactionDbContext context, string connectionString)
    {
        _context = context;
        _connectionString = connectionString;
    }

    public async Task<DeptBudget?> GetDeptBudgetAsync(
        long locationId, long deptId, long finYearId, CancellationToken ct = default)
    {
        return await _context.DeptBudgets
            .FirstOrDefaultAsync(b =>
                b.LocationId == locationId &&
                b.DeptId == deptId &&
                b.FinYearId == finYearId, ct);
    }

    public async Task<UnitBudget?> GetUnitBudgetAsync(
        long locationId, string unitCode, long finYearId, CancellationToken ct = default)
    {
        return await _context.UnitBudgets
            .FirstOrDefaultAsync(b =>
                b.LocationId == locationId &&
                b.FinYearId == finYearId, ct);
    }

    public async Task<IEnumerable<DeptBudget>> GetDeptBudgetsByLocationAsync(
        long locationId, long finYearId, CancellationToken ct = default)
    {
        return await _context.DeptBudgets
            .Where(b => b.LocationId == locationId && b.FinYearId == finYearId)
            .ToListAsync(ct);
    }

    public async Task<IEnumerable<UnitBudget>> GetUnitBudgetsByLocationAsync(
        long locationId, long finYearId, CancellationToken ct = default)
    {
        return await _context.UnitBudgets
            .Where(b => b.LocationId == locationId && b.FinYearId == finYearId)
            .ToListAsync(ct);
    }

    public async Task<long> GetRemainingBudgetSpAsync(
        long locationId, long deptId, long finYearId, CancellationToken ct = default)
    {
        var budget = await _context.DeptBudgets
            .Where(b => b.LocationId == locationId && b.DeptId == deptId && b.FinYearId == finYearId)
            .Select(b => b.BudgetAmount.Amount)
            .FirstOrDefaultAsync(ct);

        var spent = await _context.Set<Domain.Entities.OrderSub>()
            .Where(os => _context.Set<Domain.Entities.OrderMain>()
                .Any(om => om.OrderMainId == os.OrderMainId && om.LocationId == locationId))
            .Where(os => _context.Set<Domain.Entities.RequestSub>()
                .Any(rs => rs.RequestSubId == os.RequestSubId && rs.DeptId == deptId))
            .SumAsync(os => os.OrderPrice, ct);

        return budget - spent;
    }

    public async Task AddDeptBudgetAsync(DeptBudget budget, CancellationToken ct = default)
        => await _context.DeptBudgets.AddAsync(budget, ct);

    public async Task AddUnitBudgetAsync(UnitBudget budget, CancellationToken ct = default)
        => await _context.UnitBudgets.AddAsync(budget, ct);

    public void UpdateDeptBudget(DeptBudget budget)
        => _context.DeptBudgets.Update(budget);

    public void UpdateUnitBudget(UnitBudget budget)
        => _context.UnitBudgets.Update(budget);
}
