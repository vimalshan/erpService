using LoanService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LoanService.Infrastructure.Persistence;

public class LoanDbContext : DbContext
{
    public DbSet<LoanMain> Loans => Set<LoanMain>();
    public DbSet<LoanRepayment> Repayments => Set<LoanRepayment>();
    public DbSet<LoanDeduction> Deductions => Set<LoanDeduction>();

    public LoanDbContext(DbContextOptions<LoanDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
