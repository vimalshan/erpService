using Microsoft.EntityFrameworkCore;
using travelTransactionService.Domain.Common;
using travelTransactionService.Domain.Entities;

namespace travelTransactionService.Infrastructure.Data;

public class TransactionDbContext : DbContext
{
    public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options) { }

    public DbSet<VendorMaster> VendorMasters => Set<VendorMaster>();
    public DbSet<AccountMaster> AccountMasters => Set<AccountMaster>();
    public DbSet<GlCodeCombination> GlCodeCombinations => Set<GlCodeCombination>();
    public DbSet<TaxMaster> TaxMasters => Set<TaxMaster>();
    public DbSet<TaxComponent> TaxComponents => Set<TaxComponent>();
    public DbSet<JvInterface> JvInterfaces => Set<JvInterface>();
    public DbSet<JvMissingCombiCode> JvMissingCombiCodes => Set<JvMissingCombiCode>();
    public DbSet<JaiInterfaceLine> JaiInterfaceLines => Set<JaiInterfaceLine>();
    public DbSet<JaiInterfaceTaxLine> JaiInterfaceTaxLines => Set<JaiInterfaceTaxLine>();
    public DbSet<BatchSubBreakup> BatchSubBreakups => Set<BatchSubBreakup>();
    public DbSet<TravelApParams> TravelApParams => Set<TravelApParams>();
    public DbSet<SourceHistory> SourceHistories => Set<SourceHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TransactionDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
