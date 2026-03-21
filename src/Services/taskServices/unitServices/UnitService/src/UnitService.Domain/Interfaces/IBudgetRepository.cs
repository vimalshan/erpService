using UnitService.Domain.Entities;

namespace UnitService.Domain.Interfaces;

public interface IBudgetRepository
{
    Task<BudgetMaster?> GetByUnitCodeAsync(string unitCode, CancellationToken ct = default);
    Task<IEnumerable<BudgetMaster>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(BudgetMaster budget, CancellationToken ct = default);
    void Update(BudgetMaster budget);
}
