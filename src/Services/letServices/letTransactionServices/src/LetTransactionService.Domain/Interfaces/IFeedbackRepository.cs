using LetTransactionService.Domain.Entities;

namespace LetTransactionService.Domain.Interfaces;

public interface IFeedbackRepository
{
    Task<CourseFeedbackMain?> GetByIdAsync(long feedbackNumber, CancellationToken ct = default);
    Task<IEnumerable<CourseFeedbackMain>> GetByNominationAsync(long nominationNumber, CancellationToken ct = default);
    Task<IEnumerable<CourseFeedbackMain>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task AddAsync(CourseFeedbackMain feedback, CancellationToken ct = default);
    Task UpdateAsync(CourseFeedbackMain feedback, CancellationToken ct = default);
    Task<bool> ExistsAsync(long feedbackNumber, CancellationToken ct = default);
}
