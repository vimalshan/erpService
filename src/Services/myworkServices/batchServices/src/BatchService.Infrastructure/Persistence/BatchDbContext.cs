using Microsoft.EntityFrameworkCore;
using BatchService.Domain.Entities;

namespace BatchService.Infrastructure.Persistence;

public class BatchDbContext : DbContext
{
    public BatchDbContext(DbContextOptions<BatchDbContext> options) : base(options) { }

    public DbSet<BatchMaster> BatchMasters => Set<BatchMaster>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BatchDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
