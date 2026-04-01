using LetTransactionService.Application.Interfaces;
using LetTransactionService.Domain.Common;
using LetTransactionService.Domain.Entities;
using LetTransactionService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LetTransactionService.Infrastructure.Data;

public class LetTransactionDbContext(
    DbContextOptions<LetTransactionDbContext> options,
    IDomainEventDispatcher? eventDispatcher = null)
    : DbContext(options), IUnitOfWork
{
    public DbSet<LetMain> LetMain => Set<LetMain>();
    public DbSet<LetSub> LetSub => Set<LetSub>();
    public DbSet<CourseFeedbackMain> CourseFeedbackMain => Set<CourseFeedbackMain>();
    public DbSet<CourseFeedbackSub> CourseFeedbackSub => Set<CourseFeedbackSub>();
    public DbSet<ReviewMain> ReviewMain => Set<ReviewMain>();
    public DbSet<ReviewSub> ReviewSub => Set<ReviewSub>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LetTransactionDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // Collect domain events before save
        var domainEntities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Count > 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.DomainEvents)
            .OfType<DomainEvent>()
            .ToList();

        // Clear events before save to avoid re-dispatch
        domainEntities.ForEach(e => e.ClearDomainEvents());

        var result = await base.SaveChangesAsync(ct);

        // Dispatch events after successful save
        if (eventDispatcher is not null && domainEvents.Count > 0)
        {
            try
            {
                await eventDispatcher.DispatchAsync(domainEvents, ct);
            }
            catch (Exception)
            {
                // Don't fail the save because of event dispatch failure
                // Events will be lost but the data is consistent
            }
        }

        return result;
    }
}
