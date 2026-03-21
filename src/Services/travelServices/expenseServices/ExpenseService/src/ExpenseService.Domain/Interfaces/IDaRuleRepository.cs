using ExpenseService.Domain.Entities;

namespace ExpenseService.Domain.Interfaces;

public interface IDaRuleRepository
{
    Task<DaRule?> GetByIdAsync(long serialNumber, CancellationToken ct = default);
    Task<IReadOnlyList<DaRule>> GetActiveRulesAsync(long bandId, CancellationToken ct = default);
    Task<DaRule> AddAsync(DaRule rule, CancellationToken ct = default);
    Task UpdateAsync(DaRule rule, CancellationToken ct = default);
}
