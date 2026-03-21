using Microsoft.EntityFrameworkCore;
using SalesOrderService.Domain.Entities;

namespace SalesOrderService.Infrastructure.Persistence;

public class SalesOrderDbContext(DbContextOptions<SalesOrderDbContext> options) : DbContext(options)
{
    public DbSet<SalesOrder> SalesOrders { get; set; } = null!;
    public DbSet<SalesOrderLine> SalesOrderLines { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SalesOrderDbContext).Assembly);
    }
}
