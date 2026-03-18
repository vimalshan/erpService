using Microsoft.EntityFrameworkCore;
using HRService.Domain.Entities;
using HRService.Infrastructure.Data.Configurations;

namespace HRService.Infrastructure.Data;

public class HRServiceDbContext : DbContext
{
    public HRServiceDbContext(DbContextOptions<HRServiceDbContext> options)
        : base(options)
    {
    }

    public DbSet<Department> Departments { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<LeaveType> LeaveTypes { get; set; }
    public DbSet<EmployeeLeave> EmployeeLeaves { get; set; }
    public DbSet<Shift> Shifts { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<SalaryComponent> SalaryComponents { get; set; }
    public DbSet<EmployeeSalary> EmployeeSalaries { get; set; }
    public DbSet<PerformanceReview> PerformanceReviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations
        modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
        modelBuilder.ApplyConfiguration(new PositionConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
        modelBuilder.ApplyConfiguration(new LeaveTypeConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeLeaveConfiguration());
        modelBuilder.ApplyConfiguration(new ShiftConfiguration());
        modelBuilder.ApplyConfiguration(new AttendanceConfiguration());
        modelBuilder.ApplyConfiguration(new SalaryComponentConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeSalaryConfiguration());
        modelBuilder.ApplyConfiguration(new PerformanceReviewConfiguration());

        // Add global query filters
        modelBuilder.Entity<Department>().HasQueryFilter(d => d.IsActive);
        modelBuilder.Entity<Employee>().HasQueryFilter(e => !e.TerminationDate.HasValue || e.TerminationDate > DateTime.UtcNow.AddYears(-1));
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Update modification timestamps
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is Domain.Common.Entity entity)
            {
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    entity.ModifiedDate = DateTime.UtcNow;
                }
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
