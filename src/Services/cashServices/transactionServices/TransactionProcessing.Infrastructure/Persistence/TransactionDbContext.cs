using Microsoft.EntityFrameworkCore;
using TransactionProcessing.Domain.Entities;

namespace TransactionProcessing.Infrastructure.Persistence;

public class TransactionDbContext(DbContextOptions<TransactionDbContext> options) : DbContext(options)
{
    public DbSet<FinancialTransaction> FinancialTransactions => Set<FinancialTransaction>();
    public DbSet<TransactionBatch> TransactionBatches => Set<TransactionBatch>();
    public DbSet<DealSettlement> DealSettlements => Set<DealSettlement>();
    public DbSet<LoanDisbursement> LoanDisbursements => Set<LoanDisbursement>();
    public DbSet<LoanRepayment> LoanRepayments => Set<LoanRepayment>();
    public DbSet<TransactionAudit> TransactionAudits => Set<TransactionAudit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dbo");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TransactionDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
