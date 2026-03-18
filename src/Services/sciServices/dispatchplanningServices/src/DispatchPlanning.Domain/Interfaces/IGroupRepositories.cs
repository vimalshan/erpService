using DispatchPlanning.Domain.Entities;

namespace DispatchPlanning.Domain.Interfaces;

public interface IDispatchPlanMainGroupRepository
{
    Task<DispatchPlanMainGroup?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<DispatchPlanMainGroup>> GetAllAsync(int companyUnitId, CancellationToken ct = default);
    Task AddAsync(DispatchPlanMainGroup group, CancellationToken ct = default);
    Task UpdateAsync(DispatchPlanMainGroup group, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}

public interface IDispatchPlanSubGroupRepository
{
    Task<DispatchPlanSubGroup?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<DispatchPlanSubGroup>> GetByMainGroupAsync(int mainGroupId, CancellationToken ct = default);
    Task AddAsync(DispatchPlanSubGroup subGroup, CancellationToken ct = default);
    Task UpdateAsync(DispatchPlanSubGroup subGroup, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}

public interface IDispatchPlanBreakupItemRepository
{
    Task<DispatchPlanBreakupItem?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<DispatchPlanBreakupItem>> GetBySubGroupAsync(int subGroupId, CancellationToken ct = default);
    Task AddAsync(DispatchPlanBreakupItem item, CancellationToken ct = default);
    Task UpdateAsync(DispatchPlanBreakupItem item, CancellationToken ct = default);
    Task DeleteAsync(int id, int deletedBy, CancellationToken ct = default);
}
