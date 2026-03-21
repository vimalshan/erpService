using Microsoft.EntityFrameworkCore;
using SupplierService.Domain.Entities;

namespace SupplierService.Infrastructure.Persistence;

public class SupplierDbContext : DbContext
{
    public DbSet<Supplier> Suppliers => Set<Supplier>();

    public SupplierDbContext(DbContextOptions<SupplierDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SupplierDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
