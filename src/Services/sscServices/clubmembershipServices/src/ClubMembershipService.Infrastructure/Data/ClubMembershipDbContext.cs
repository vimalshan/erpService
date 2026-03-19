using Microsoft.EntityFrameworkCore;
using ClubMembershipService.Domain.Common;
using ClubMembershipService.Domain.Entities;

namespace ClubMembershipService.Infrastructure.Data;

public class ClubMembershipDbContext : DbContext
{
    public ClubMembershipDbContext(DbContextOptions<ClubMembershipDbContext> options)
        : base(options) { }

    public DbSet<ClubMaster> ClubMasters => Set<ClubMaster>();
    public DbSet<ClubMembership> ClubMemberships => Set<ClubMembership>();
    public DbSet<ClubActivity> ClubActivities => Set<ClubActivity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClubMembershipDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var entity in entitiesWithEvents)
            entity.ClearDomainEvents();

        return result;
    }
}
