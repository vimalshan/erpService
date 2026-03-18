using DispatchPlanning.Domain.Aggregates;

namespace DispatchPlanning.Domain.Interfaces;

public interface IDispatchPlanRepository
{
    Task<DispatchPlanAggregate?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<DispatchPlanAggregate>> GetAllAsync(int companyUnitId, CancellationToken ct = default);
    Task<int> AddAsync(DispatchPlanAggregate plan, CancellationToken ct = default);
    Task UpdateAsync(DispatchPlanAggregate plan, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
