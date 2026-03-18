using Microsoft.EntityFrameworkCore;
using LovService.Domain.Entities;

namespace LovService.Infrastructure.Data;

public class LovDbContext(DbContextOptions<LovDbContext> options) : DbContext(options)
{
    public DbSet<LovTypeMast> LovTypeMasts => Set<LovTypeMast>();
    public DbSet<LovMaster> LovMasters => Set<LovMaster>();
    public DbSet<ProgramLovMast> ProgramLovMasts => Set<ProgramLovMast>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LovDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
