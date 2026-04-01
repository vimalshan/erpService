using LetTransactionService.Domain.Entities;

namespace LetTransactionService.Domain.Interfaces;

public interface IReviewRepository
{
    Task<ReviewMain?> GetByIdAsync(long reviewSerialNumber, CancellationToken ct = default);
    Task<IEnumerable<ReviewMain>> GetByFeedbackAsync(long feedbackNumber, CancellationToken ct = default);
    Task<IEnumerable<ReviewMain>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task<IEnumerable<ReviewMain>> GetPendingReviewsAsync(CancellationToken ct = default);
    Task AddAsync(ReviewMain review, CancellationToken ct = default);
    Task UpdateAsync(ReviewMain review, CancellationToken ct = default);
    Task<bool> ExistsAsync(long reviewSerialNumber, CancellationToken ct = default);
}
