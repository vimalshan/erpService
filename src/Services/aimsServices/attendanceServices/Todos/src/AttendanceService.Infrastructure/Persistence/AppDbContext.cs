using AttendanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AttendanceService.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<SwipeRawPunch> SwipeRawPunches => Set<SwipeRawPunch>();
    public DbSet<SwipeRawPunchLog> SwipeRawPunchLogs => Set<SwipeRawPunchLog>();
    public DbSet<AttendanceBatch> AttendanceBatches => Set<AttendanceBatch>();
    public DbSet<AttendanceSummary> AttendanceSummaries => Set<AttendanceSummary>();
    public DbSet<AttendanceOvertime> AttendanceOvertimes => Set<AttendanceOvertime>();
    public DbSet<AttendanceNight> AttendanceNights => Set<AttendanceNight>();
    public DbSet<AttendanceLopMain> AttendanceLopMains => Set<AttendanceLopMain>();
    public DbSet<AttendanceGraceAdjust> AttendanceGraceAdjusts => Set<AttendanceGraceAdjust>();
    public DbSet<AttendanceLeaveAdjust> AttendanceLeaveAdjusts => Set<AttendanceLeaveAdjust>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
