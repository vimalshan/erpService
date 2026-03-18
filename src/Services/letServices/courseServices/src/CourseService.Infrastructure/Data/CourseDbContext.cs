using CourseService.Domain.Aggregates;
using CourseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseService.Infrastructure.Data;

public class CourseDbContext(DbContextOptions<CourseDbContext> options) : DbContext(options)
{
    public DbSet<CourseAggregate> Courses => Set<CourseAggregate>();
    public DbSet<CourseSchedule> CourseSchedules => Set<CourseSchedule>();
    public DbSet<CourseParticipant> CourseParticipants => Set<CourseParticipant>();
    public DbSet<CourseBand> CourseBands => Set<CourseBand>();
    public DbSet<CourseCost> CourseCosts => Set<CourseCost>();
    public DbSet<CourseModel> CourseModels => Set<CourseModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CourseDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
