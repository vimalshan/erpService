using FaqServices.Domain.Entities;

namespace FaqServices.Domain.Interfaces;

public interface IFaqAnswerRepository
{
    Task<FaqAnswer?> GetByIdAsync(string id, CancellationToken ct = default);
    Task<IEnumerable<FaqAnswer>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<FaqAnswer>> GetByQuestionIdAsync(string questionId, CancellationToken ct = default);
    Task<IEnumerable<FaqAnswer>> GetActiveByQuestionIdAsync(string questionId, CancellationToken ct = default);
    Task<IEnumerable<FaqAnswer>> GetCorrectAnswersForQuestionAsync(string questionId, CancellationToken ct = default);
    Task AddAsync(FaqAnswer answer, CancellationToken ct = default);
    void Update(FaqAnswer answer);
    void Remove(FaqAnswer answer);
    Task<bool> ExistsAsync(string id, CancellationToken ct = default);
}
