using ReviewService.Domain.Entities;

namespace ReviewService.Domain.Interfaces;

public interface IFeedbackRepository
{
    Task<CourseFeedMain?> GetByCompositeKeyAsync(string userId, long courseId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CourseFeedMain>> GetByCourseIdAsync(long courseId, CancellationToken cancellationToken = default);
    Task<(int TotalFeedbacks, decimal AverageRating)> GetFeedbackSummaryAsync(long courseId, CancellationToken cancellationToken = default);
    Task AddAsync(CourseFeedMain feedback, CancellationToken cancellationToken = default);
    Task AddSubAsync(CourseFeedSub feedSub, CancellationToken cancellationToken = default);
    Task UpdateAsync(CourseFeedMain feedback, CancellationToken cancellationToken = default);
}
