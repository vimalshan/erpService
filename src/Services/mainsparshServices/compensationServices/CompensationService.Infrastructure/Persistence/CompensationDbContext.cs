using Microsoft.EntityFrameworkCore;
using CompensationService.Domain.Entities;
using CompensationService.Domain.Common;
using CompensationService.Infrastructure.Persistence.EntityConfigurations;

namespace CompensationService.Infrastructure.Persistence;

/// <summary>
/// DbContext for Compensation Service
/// </summary>
public class CompensationDbContext : DbContext
{
    public DbSet<CompensationGrade> CompensationGrades { get; set; } = null!;

    public CompensationDbContext(DbContextOptions<CompensationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<DomainEvent>();
        modelBuilder.ApplyConfiguration(new CompensationGradeConfiguration());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        await DispatchDomainEventsAsync();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEventsAsync()
    {
        var aggregateRoots = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();

        foreach (var aggregate in aggregateRoots)
        {
            aggregate.ClearDomainEvents();
        }

        // In a real application, you would publish these events to a message bus
        // For now, we'll just clear them
    }
}
