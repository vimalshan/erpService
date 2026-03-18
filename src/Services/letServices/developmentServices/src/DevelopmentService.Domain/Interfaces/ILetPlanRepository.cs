using DevelopmentService.Domain.Entities;

namespace DevelopmentService.Domain.Interfaces;

public interface ILetPlanRepository
{
    Task<LetPlan?> GetByIdAsync(long reqNum, CancellationToken ct = default);
    Task<IEnumerable<LetPlan>> GetAllAsync(string? userId, char? status, CancellationToken ct = default);
    Task AddAsync(LetPlan plan, CancellationToken ct = default);
    Task UpdateAsync(LetPlan plan, CancellationToken ct = default);
    Task DeleteAsync(long reqNum, CancellationToken ct = default);
}
