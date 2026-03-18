using Microsoft.EntityFrameworkCore;
using TimesheetService.Domain.Entities;
using TimesheetService.Infrastructure.Data.Configurations;

namespace TimesheetService.Infrastructure.Data;

public sealed class TimesheetDbContext : DbContext
{
    public TimesheetDbContext(DbContextOptions<TimesheetDbContext> options) : base(options) { }

    public DbSet<Timesheet> Timesheets => Set<Timesheet>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new TimesheetConfiguration());
    }
}
