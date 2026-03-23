using Microsoft.EntityFrameworkCore;
using ReferenceService.Domain;
using ReferenceService.Domain.Entities;

namespace ReferenceService.Infrastructure.Persistence;

/// <summary>
/// EF Core DbContext for Reference Service.
/// </summary>
public class ReferenceDbContext : DbContext
{
    public ReferenceDbContext(DbContextOptions<ReferenceDbContext> options) : base(options) { }
    
    public DbSet<LovType> LovTypes => Set<LovType>();
    public DbSet<LovValue> LovValues => Set<LovValue>();
    public DbSet<PermissionRule> PermissionRules => Set<PermissionRule>();
    public DbSet<LeaveFlag> LeaveFlags => Set<LeaveFlag>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Ignore domain events - they are in-memory only
        modelBuilder.Ignore<DomainEvent>();
        
        // Apply configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReferenceDbContext).Assembly);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var domainEntities = ChangeTracker
            .Entries<Entity<int>>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList();
        
        foreach (var entry in domainEntities)
        {
            entry.Entity.ClearDomainEvents();
        }
        
        return await base.SaveChangesAsync(cancellationToken);
    }
}
