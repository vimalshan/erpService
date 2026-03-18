using CourseService.Domain.Entities;

namespace CourseService.Domain.Interfaces;

public interface ICourseScheduleRepository
{
    Task<IEnumerable<CourseSchedule>> GetByCourseIdAsync(long courseId, CancellationToken ct = default);
    Task AddAsync(CourseSchedule schedule, CancellationToken ct = default);
    Task DeleteByCourseIdAsync(long courseId, CancellationToken ct = default);
}

public interface ICourseParticipantRepository
{
    Task<IEnumerable<CourseParticipant>> GetByCourseIdAsync(long courseId, CancellationToken ct = default);
    Task<CourseParticipant?> GetByUserCodeAsync(long courseId, string userCode, CancellationToken ct = default);
    Task AddAsync(CourseParticipant participant, CancellationToken ct = default);
    Task UpdateAsync(CourseParticipant participant, CancellationToken ct = default);
}

public interface IBlobStorageService
{
    Task<string> UploadAsync(Stream stream, string fileName, string containerName, CancellationToken ct = default);
    Task<Stream> DownloadAsync(string fileName, string containerName, CancellationToken ct = default);
    Task DeleteAsync(string fileName, string containerName, CancellationToken ct = default);
}
