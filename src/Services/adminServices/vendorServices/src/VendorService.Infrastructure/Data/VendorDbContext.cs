using Microsoft.EntityFrameworkCore;
using VendorService.Domain.Common;
using VendorService.Domain.Entities;

namespace VendorService.Infrastructure.Data;

public sealed class VendorDbContext : DbContext
{
    public VendorDbContext(DbContextOptions<VendorDbContext> options) : base(options) { }

    public DbSet<VendorMaster> VendorMasters => Set<VendorMaster>();
    public DbSet<TdsVendor> TdsVendors => Set<TdsVendor>();
    public DbSet<TdsFileDetail> TdsFileDetails => Set<TdsFileDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VendorDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch domain events before saving
        var aggregates = ChangeTracker.Entries<Entity>()
            .Where(e => e.Entity.DomainEvents.Count > 0)
            .Select(e => e.Entity)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var aggregate in aggregates)
            aggregate.ClearDomainEvents();

        return result;
    }
}
