using FaqServices.Domain.Entities;

namespace FaqServices.Domain.Interfaces;

public interface IFaqQuestionRepository
{
    Task<FaqQuestion?> GetByIdAsync(string id, CancellationToken ct = default);
    Task<FaqQuestion?> GetByIdWithAnswersAsync(string id, CancellationToken ct = default);
    Task<IEnumerable<FaqQuestion>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<FaqQuestion>> GetByGradeIdAsync(string gradeId, CancellationToken ct = default);
    Task<IEnumerable<FaqQuestion>> GetActiveByGradeIdAsync(string gradeId, CancellationToken ct = default);
    Task AddAsync(FaqQuestion question, CancellationToken ct = default);
    void Update(FaqQuestion question);
    void Remove(FaqQuestion question);
    Task<bool> ExistsAsync(string id, CancellationToken ct = default);
}
