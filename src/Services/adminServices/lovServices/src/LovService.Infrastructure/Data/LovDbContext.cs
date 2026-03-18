using LovService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LovService.Infrastructure.Data;

public class LovDbContext(DbContextOptions<LovDbContext> options) : DbContext(options)
{
    public DbSet<LovType> LovTypes { get; set; }
    public DbSet<LovMaster> LovMasters { get; set; }
    public DbSet<ItemData> ItemDataSet { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LovDbContext).Assembly);
    }
}
