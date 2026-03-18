using Microsoft.EntityFrameworkCore;
using OtherService.Domain.Entities;

namespace OtherService.Infrastructure.Persistence;

public sealed class OtherDbContext : DbContext
{
    public OtherDbContext(DbContextOptions<OtherDbContext> options) : base(options) { }

    public DbSet<LogDdCatDevDetail> LogDdCatDevDetails => Set<LogDdCatDevDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OtherDbContext).Assembly);
    }
}
