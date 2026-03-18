using MasterDataService.Domain.Common;
using MasterDataService.Domain.Entities;
using MasterDataService.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MasterDataService.Infrastructure.Persistence;

public class MasterDataDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    public MasterDataDbContext(DbContextOptions<MasterDataDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<FinancialYearRule> FinancialYearRules => Set<FinancialYearRule>();
    public DbSet<FundType> FundTypes => Set<FundType>();
    public DbSet<LovMaster> LovMasters => Set<LovMaster>();
    public DbSet<StatusMaster> StatusMasters => Set<StatusMaster>();
    public DbSet<RoleMaster> RoleMasters => Set<RoleMaster>();
    public DbSet<RateType> RateTypes => Set<RateType>();
    public DbSet<RateMaster> RateMasters => Set<RateMaster>();
    public DbSet<ComputationMonth> ComputationMonths => Set<ComputationMonth>();
    public DbSet<ComputationFinancialYear> ComputationFinancialYears => Set<ComputationFinancialYear>();
    public DbSet<InvestmentCategoryGroup> InvestmentCategoryGroups => Set<InvestmentCategoryGroup>();
    public DbSet<InvestmentCategoryLimit> InvestmentCategoryLimits => Set<InvestmentCategoryLimit>();
    public DbSet<InvestmentGroupLimit> InvestmentGroupLimits => Set<InvestmentGroupLimit>();
    public DbSet<PfHris> PfHrisRecords => Set<PfHris>();
    public DbSet<PfMainAccount> PfMainAccounts => Set<PfMainAccount>();
    public DbSet<PfMainSubMapping> PfMainSubMappings => Set<PfMainSubMapping>();
    public DbSet<Configuration> Configurations => Set<Configuration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MasterDataDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var entity in entities)
        {
            var events = entity.DomainEvents.ToList();
            entity.ClearDomainEvents();
            foreach (var domainEvent in events)
                await _mediator.Publish(domainEvent, cancellationToken);
        }

        return result;
    }
}
