using Microsoft.EntityFrameworkCore;
using StrategicStock.Domain.Entities;

namespace StrategicStock.Infrastructure.Persistence;

public sealed class StrategicStockDbContext(DbContextOptions<StrategicStockDbContext> options)
    : DbContext(options)
{
    public DbSet<StrategicStockEntity> StrategicStocks => Set<StrategicStockEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StrategicStockDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
