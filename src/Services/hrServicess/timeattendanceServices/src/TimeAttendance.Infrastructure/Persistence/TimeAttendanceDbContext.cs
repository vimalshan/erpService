using Microsoft.EntityFrameworkCore;
using TimeAttendance.Domain.Common;
using TimeAttendance.Domain.Entities;

namespace TimeAttendance.Infrastructure.Persistence;

public class TimeAttendanceDbContext(DbContextOptions<TimeAttendanceDbContext> options)
    : DbContext(options)
{
    public DbSet<AbsenteeismDetail> AbsenteeismDetails => Set<AbsenteeismDetail>();
    public DbSet<AbsenteeismMis> AbsenteeismMisRecords => Set<AbsenteeismMis>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TimeAttendanceDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var entities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var entity in entities)
            entity.ClearDomainEvents();

        return result;
    }
}
