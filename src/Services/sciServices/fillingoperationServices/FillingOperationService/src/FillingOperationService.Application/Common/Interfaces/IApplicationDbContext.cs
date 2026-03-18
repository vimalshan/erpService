using FillingOperationService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FillingOperationService.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<FillingPlant> FillingPlants { get; }
    DbSet<FillingLine> FillingLines { get; }
    DbSet<FillingPointGroup> FillingPointGroups { get; }
    DbSet<FillingLineProductMap> FillingLineProductMaps { get; }
    DbSet<Domain.Entities.FillingCapacity> FillingCapacities { get; }
    DbSet<FlSwitchoverTime> FlSwitchoverTimes { get; }
    DbSet<FlWorkingShift> FlWorkingShifts { get; }
    DbSet<FpgDowntime> FpgDowntimes { get; }
    DbSet<PlanDeviation> PlanDeviations { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
