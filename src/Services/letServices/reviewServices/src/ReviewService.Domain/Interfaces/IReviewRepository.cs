using ReviewService.Domain.Entities;

namespace ReviewService.Domain.Interfaces;

public interface IReviewRepository
{
    Task<ReviewMain?> GetByIdAsync(long srlNum, CancellationToken cancellationToken = default);
    Task<IEnumerable<ReviewMain>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ReviewSub>> GetSubsByMainIdAsync(long mainSrl, CancellationToken cancellationToken = default);
    Task AddAsync(ReviewMain review, CancellationToken cancellationToken = default);
    Task AddSubAsync(ReviewSub reviewSub, CancellationToken cancellationToken = default);
    Task UpdateAsync(ReviewMain review, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long srlNum, CancellationToken cancellationToken = default);
}
