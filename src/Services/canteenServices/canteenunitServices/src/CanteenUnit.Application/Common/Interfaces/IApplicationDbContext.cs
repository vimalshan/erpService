using CanteenUnit.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CanteenUnit.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<CanteenUnitMaster> CanteenUnitMasters { get; }
    DbSet<CanteenMaster> CanteenMasters { get; }
    DbSet<CanteenMasterCat> CanteenMasterCats { get; }
    DbSet<CanteenMasterGradeCat> CanteenMasterGradeCats { get; }
    DbSet<CanteenUnitAccess> CanteenUnitAccesses { get; }
    DbSet<GenCounter> GenCounters { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
