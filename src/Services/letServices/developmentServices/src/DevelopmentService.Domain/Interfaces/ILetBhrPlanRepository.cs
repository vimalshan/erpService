using DevelopmentService.Domain.Entities;

namespace DevelopmentService.Domain.Interfaces;

public interface ILetBhrPlanRepository
{
    Task<LetBhrPlan?> GetByIdAsync(long reqNum, CancellationToken ct = default);
    Task AddAsync(LetBhrPlan plan, CancellationToken ct = default);
}
