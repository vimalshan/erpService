using FillingOperationService.Application.Common.Interfaces;
using FillingOperationService.Domain.Common;
using FillingOperationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FillingOperationService.Infrastructure.Persistence;

public class FillingOperationsDbContext(DbContextOptions<FillingOperationsDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<FillingPlant> FillingPlants => Set<FillingPlant>();
    public DbSet<FillingLine> FillingLines => Set<FillingLine>();
    public DbSet<FillingPointGroup> FillingPointGroups => Set<FillingPointGroup>();
    public DbSet<FillingLineProductMap> FillingLineProductMaps => Set<FillingLineProductMap>();
    public DbSet<FillingCapacity> FillingCapacities => Set<FillingCapacity>();
    public DbSet<FlSwitchoverTime> FlSwitchoverTimes => Set<FlSwitchoverTime>();
    public DbSet<FlWorkingShift> FlWorkingShifts => Set<FlWorkingShift>();
    public DbSet<FpgDowntime> FpgDowntimes => Set<FpgDowntime>();
    public DbSet<PlanDeviation> PlanDeviations => Set<PlanDeviation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
