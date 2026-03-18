using CourseService.Domain.Aggregates;

namespace CourseService.Domain.Interfaces;

public interface ICourseRepository
{
    Task<CourseAggregate?> GetByIdAsync(long courseId, CancellationToken ct = default);
    Task<IEnumerable<CourseAggregate>> GetAllAsync(int page, int pageSize, CancellationToken ct = default);
    Task<IEnumerable<CourseAggregate>> GetByCourseTypeAsync(char courseType, CancellationToken ct = default);
    Task AddAsync(CourseAggregate course, CancellationToken ct = default);
    Task UpdateAsync(CourseAggregate course, CancellationToken ct = default);
    Task DeleteAsync(long courseId, CancellationToken ct = default);
    Task<bool> ExistsAsync(long courseId, CancellationToken ct = default);
}
