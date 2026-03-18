using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using LoanAccount.Domain.Common;
using LoanAccount.Domain.Entities;
using LoanAccount.Domain.ValueObjects;
using LoanAccount.Infrastructure.Configuration;
using LoanAccount.Infrastructure.EventPublishing;

namespace LoanAccount.Infrastructure.Persistence;

/// <summary>
/// EF Core database context for loan account operations
/// </summary>
public class LoanAccountDbContext : DbContext
{
    public DbSet<LoanMain> LoanMains { get; set; }
    public DbSet<LoanInstallment> LoanInstallments { get; set; }
    public DbSet<LoanEmployeeInterestRate> LoanEmployeeInterestRates { get; set; }
    public DbSet<LoanLedger> LoanLedgers { get; set; }
    public DbSet<LoanSettlement> LoanSettlements { get; set; }

    public LoanAccountDbContext(DbContextOptions<LoanAccountDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply configurations from assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LoanAccountDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        return base.SaveChanges();
    }
}
