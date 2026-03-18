using EximManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EximManagement.Infrastructure.Data;

public class EximDbContext(DbContextOptions<EximDbContext> options, IMediator? mediator = null) : DbContext(options)
{
    public DbSet<EximDataFile> EximDataFiles => Set<EximDataFile>();
    public DbSet<EximProduct> EximProducts => Set<EximProduct>();
    public DbSet<EximProductSearch> EximProductSearches => Set<EximProductSearch>();
    public DbSet<EximProductGroup> EximProductGroups => Set<EximProductGroup>();
    public DbSet<EximProductGroupMap> EximProductGroupMaps => Set<EximProductGroupMap>();
    public DbSet<EximDataExport> EximDataExports => Set<EximDataExport>();
    public DbSet<EximDataImport> EximDataImports => Set<EximDataImport>();
    public DbSet<EximUserMaster> EximUserMasters => Set<EximUserMaster>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EximDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var entitiesWithEvents = ChangeTracker.Entries<Domain.Common.BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var result = await base.SaveChangesAsync(ct);

        if (mediator is not null)
        {
            foreach (var entity in entitiesWithEvents)
            {
                var events = entity.DomainEvents.ToList();
                entity.ClearDomainEvents();
                foreach (var domainEvent in events)
                    await mediator.Publish(domainEvent, ct);
            }
        }
        else
        {
            foreach (var entity in entitiesWithEvents)
                entity.ClearDomainEvents();
        }

        return result;
    }
}
