using CourseService.Domain.Aggregates;
using CourseService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseService.Infrastructure.Repositories;

public class CourseRepository(CourseService.Infrastructure.Data.CourseDbContext dbContext) : ICourseRepository
{
    public async Task<CourseAggregate?> GetByIdAsync(long courseId, CancellationToken ct = default)
        => await dbContext.Courses
            .Include(c => c.Schedules)
            .Include(c => c.Participants)
            .Include(c => c.Bands)
            .Include(c => c.Costs)
            .Include(c => c.Models)
            .FirstOrDefaultAsync(c => c.CourseId == courseId, ct);

    public async Task<IEnumerable<CourseAggregate>> GetAllAsync(int page, int pageSize, CancellationToken ct = default)
        => await dbContext.Courses
            .Include(c => c.Schedules)
            .Include(c => c.Participants)
            .OrderByDescending(c => c.EffectiveDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public async Task<IEnumerable<CourseAggregate>> GetByCourseTypeAsync(char courseType, CancellationToken ct = default)
        => await dbContext.Courses
            .Include(c => c.Schedules)
            .Include(c => c.Participants)
            .Where(c => c.CourseType == courseType)
            .OrderByDescending(c => c.EffectiveDate)
            .ToListAsync(ct);

    public async Task AddAsync(CourseAggregate course, CancellationToken ct = default)
    {
        await dbContext.Courses.AddAsync(course, ct);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(CourseAggregate course, CancellationToken ct = default)
    {
        dbContext.Courses.Update(course);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(long courseId, CancellationToken ct = default)
    {
        var course = await dbContext.Courses.FindAsync([courseId], ct);
        if (course is not null)
        {
            dbContext.Courses.Remove(course);
            await dbContext.SaveChangesAsync(ct);
        }
    }

    public async Task<bool> ExistsAsync(long courseId, CancellationToken ct = default)
        => await dbContext.Courses.AnyAsync(c => c.CourseId == courseId, ct);
}
