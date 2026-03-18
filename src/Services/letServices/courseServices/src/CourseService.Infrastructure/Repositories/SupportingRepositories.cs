using CourseService.Domain.Entities;
using CourseService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseService.Infrastructure.Repositories;

public class CourseScheduleRepository(CourseService.Infrastructure.Data.CourseDbContext dbContext) : ICourseScheduleRepository
{
    public async Task<IEnumerable<CourseSchedule>> GetByCourseIdAsync(long courseId, CancellationToken ct = default)
        => await dbContext.CourseSchedules.Where(s => s.CourseId == courseId).OrderBy(s => s.ScheduleDate).ToListAsync(ct);

    public async Task AddAsync(CourseSchedule schedule, CancellationToken ct = default)
    {
        await dbContext.CourseSchedules.AddAsync(schedule, ct);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteByCourseIdAsync(long courseId, CancellationToken ct = default)
    {
        var schedules = await dbContext.CourseSchedules.Where(s => s.CourseId == courseId).ToListAsync(ct);
        dbContext.CourseSchedules.RemoveRange(schedules);
        await dbContext.SaveChangesAsync(ct);
    }
}

public class CourseParticipantRepository(CourseService.Infrastructure.Data.CourseDbContext dbContext) : ICourseParticipantRepository
{
    public async Task<IEnumerable<CourseParticipant>> GetByCourseIdAsync(long courseId, CancellationToken ct = default)
        => await dbContext.CourseParticipants.Where(p => p.CourseId == courseId).ToListAsync(ct);

    public async Task<CourseParticipant?> GetByUserCodeAsync(long courseId, string userCode, CancellationToken ct = default)
        => await dbContext.CourseParticipants
            .Where(p => p.CourseId == courseId && p.UserCode == userCode)
            .FirstOrDefaultAsync(ct);

    public async Task AddAsync(CourseParticipant participant, CancellationToken ct = default)
    {
        await dbContext.CourseParticipants.AddAsync(participant, ct);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(CourseParticipant participant, CancellationToken ct = default)
    {
        dbContext.CourseParticipants.Update(participant);
        await dbContext.SaveChangesAsync(ct);
    }
}
