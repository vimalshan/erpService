namespace TransactionService.Domain.Interfaces;

using TransactionService.Domain.Entities;

public interface IBudgetRepository
{
    Task<DeptBudget?> GetDeptBudgetAsync(long locationId, long deptId, long finYearId, CancellationToken ct = default);
    Task<UnitBudget?> GetUnitBudgetAsync(long locationId, string unitCode, long finYearId, CancellationToken ct = default);
    Task<IEnumerable<DeptBudget>> GetDeptBudgetsByLocationAsync(long locationId, long finYearId, CancellationToken ct = default);
    Task<IEnumerable<UnitBudget>> GetUnitBudgetsByLocationAsync(long locationId, long finYearId, CancellationToken ct = default);
    Task<long> GetRemainingBudgetSpAsync(long locationId, long deptId, long finYearId, CancellationToken ct = default);
    Task AddDeptBudgetAsync(DeptBudget budget, CancellationToken ct = default);
    Task AddUnitBudgetAsync(UnitBudget budget, CancellationToken ct = default);
    void UpdateDeptBudget(DeptBudget budget);
    void UpdateUnitBudget(UnitBudget budget);
}
