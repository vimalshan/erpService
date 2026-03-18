using LoanDefinition.Domain.Entities;
using LoanDefinition.SharedKernel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LoanDefinition.Infrastructure.Persistence;

public class LoanDefinitionDbContext(DbContextOptions<LoanDefinitionDbContext> options, IMediator mediator)
    : DbContext(options), IUnitOfWork
{
    public DbSet<LoanTypeMaster> LoanTypeMasters => Set<LoanTypeMaster>();
    public DbSet<LoanMaster> LoanMasters => Set<LoanMaster>();
    public DbSet<LoanSubClass> LoanSubClasses => Set<LoanSubClass>();
    public DbSet<LoanInterestRateMaster> LoanInterestRates => Set<LoanInterestRateMaster>();
    public DbSet<LoanLimitRangeMaster> LoanLimitRanges => Set<LoanLimitRangeMaster>();
    public DbSet<LoanPerquisite> LoanPerquisites => Set<LoanPerquisite>();
    public DbSet<LoanFestival> LoanFestivals => Set<LoanFestival>();
    public DbSet<LoanFestivalMap> LoanFestivalMaps => Set<LoanFestivalMap>();
    public DbSet<LoanAccountMaster> LoanAccountMasters => Set<LoanAccountMaster>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LoanDefinitionDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var domainEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = domainEntities.SelectMany(e => e.DomainEvents).ToList();

        domainEntities.ForEach(e => e.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent, cancellationToken);
        }

        return result;
    }
}
