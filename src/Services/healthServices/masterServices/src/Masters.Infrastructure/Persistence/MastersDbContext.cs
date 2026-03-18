using Microsoft.EntityFrameworkCore;
using Masters.Domain.Entities;
using Masters.Infrastructure.Persistence.Configurations;

namespace Masters.Infrastructure.Persistence;

public class MastersDbContext : DbContext
{
    public MastersDbContext(DbContextOptions<MastersDbContext> options) : base(options)
    {
    }

    public DbSet<LovTypeMaster> LovTypeMasters => Set<LovTypeMaster>();
    public DbSet<LovMaster> LovMasters => Set<LovMaster>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new LovTypeMasterConfiguration());
        modelBuilder.ApplyConfiguration(new LovMasterConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
