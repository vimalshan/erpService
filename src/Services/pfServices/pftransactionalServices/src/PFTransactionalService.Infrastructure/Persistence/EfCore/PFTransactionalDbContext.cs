using Microsoft.EntityFrameworkCore;
using PFTransactionalService.Domain.Aggregates;
using PFTransactionalService.Domain.Entities;

namespace PFTransactionalService.Infrastructure.Persistence.EfCore;

public class PFTransactionalDbContext : DbContext
{
    public DbSet<PFAccumulation> PFAccumulations => Set<PFAccumulation>();
    public DbSet<PFContributionTxn> PFContributionTxns => Set<PFContributionTxn>();
    public DbSet<PFSettlement> PFSettlements => Set<PFSettlement>();
    public DbSet<PFSettlementTxn> PFSettlementTxns => Set<PFSettlementTxn>();
    public DbSet<PFWithdrawalCertificate> PFWithdrawalCertificates => Set<PFWithdrawalCertificate>();
    public DbSet<FinancialYear> FinancialYears => Set<FinancialYear>();
    public DbSet<TransactionMaster> TransactionMasters => Set<TransactionMaster>();

    public PFTransactionalDbContext(DbContextOptions<PFTransactionalDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PFTransactionalDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
