using LoanManagement.Domain.Entities;
using LoanManagement.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Infrastructure.Data;

public class LoanManagementDbContext : DbContext, IUnitOfWork
{
    public LoanManagementDbContext(DbContextOptions<LoanManagementDbContext> options) : base(options) { }

    public DbSet<LoanMain> LoanMain { get; set; }
    public DbSet<LoanDisbursementSchedule> LoanDisbursementSchedules { get; set; }
    public DbSet<LoanInterest> LoanInterests { get; set; }
    public DbSet<LoanRepaymentSchedule> LoanRepaymentSchedules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LoanManagementDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
