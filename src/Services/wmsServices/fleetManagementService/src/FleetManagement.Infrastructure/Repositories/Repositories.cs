using FleetManagement.Domain.Common;
using FleetManagement.Domain.Entities;
using FleetManagement.Domain.Enums;
using FleetManagement.Domain.Interfaces;
using FleetManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetManagement.Infrastructure.Repositories;

public class Repository<T>(FleetDbContext db) : IRepository<T> where T : BaseEntity
{
    protected readonly FleetDbContext Db = db;

    public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        => await Db.Set<T>().FindAsync([id], ct);

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default)
        => await Db.Set<T>().AsNoTracking().ToListAsync(ct);

    public async Task<T> AddAsync(T entity, CancellationToken ct = default)
    {
        await Db.Set<T>().AddAsync(entity, ct);
        return entity;
    }

    public Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        Db.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity, CancellationToken ct = default)
    {
        Db.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }
}

public class VehicleRepository(FleetDbContext db) : Repository<Vehicle>(db), IVehicleRepository
{
    public async Task<Vehicle?> GetByCodeAsync(string code, CancellationToken ct = default)
        => await Db.Vehicles.FirstOrDefaultAsync(v => v.Code == code, ct);

    public async Task<IReadOnlyList<Vehicle>> GetByStatusAsync(VehicleStatus status, CancellationToken ct = default)
        => await Db.Vehicles.Where(v => v.Status == status).AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<Vehicle>> GetByWarehouseAsync(int warehouseId, CancellationToken ct = default)
        => await Db.Vehicles.Where(v => v.WarehouseId == warehouseId).AsNoTracking().ToListAsync(ct);
}

public class DriverRepository(FleetDbContext db) : Repository<Driver>(db), IDriverRepository
{
    public async Task<Driver?> GetByCodeAsync(string code, CancellationToken ct = default)
        => await Db.Drivers.FirstOrDefaultAsync(d => d.Code == code, ct);

    public async Task<IReadOnlyList<Driver>> GetActiveDriversAsync(CancellationToken ct = default)
        => await Db.Drivers.Where(d => d.IsActive).AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<Driver>> GetDriversWithExpiringLicensesAsync(int daysThreshold, CancellationToken ct = default)
    {
        var threshold = DateTime.UtcNow.AddDays(daysThreshold);
        return await Db.Drivers.Where(d => d.IsActive && d.LicenseExpiry <= threshold).AsNoTracking().ToListAsync(ct);
    }
}

public class RouteRepository(FleetDbContext db) : Repository<Route>(db), IRouteRepository
{
    public async Task<Route?> GetByNameAsync(string routeName, CancellationToken ct = default)
        => await Db.Routes.FirstOrDefaultAsync(r => r.RouteName == routeName, ct);
}

public class TripRepository(FleetDbContext db) : Repository<Trip>(db), ITripRepository
{
    public async Task<Trip?> GetByTripNumberAsync(string tripNumber, CancellationToken ct = default)
        => await Db.Trips.FirstOrDefaultAsync(t => t.TripNumber == tripNumber, ct);

    public async Task<Trip?> GetWithStopsAsync(int tripId, CancellationToken ct = default)
        => await Db.Trips.Include(t => t.Stops.OrderBy(s => s.StopSequence))
            .Include(t => t.Vehicle).Include(t => t.Driver).Include(t => t.Route)
            .FirstOrDefaultAsync(t => t.TripId == tripId, ct);

    public async Task<IReadOnlyList<Trip>> GetByVehicleAsync(int vehicleId, CancellationToken ct = default)
        => await Db.Trips.Where(t => t.VehicleId == vehicleId).AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<Trip>> GetByDriverAsync(int driverId, CancellationToken ct = default)
        => await Db.Trips.Where(t => t.DriverId == driverId).AsNoTracking().ToListAsync(ct);

    public async Task<IReadOnlyList<Trip>> GetByStatusAsync(TripStatus status, CancellationToken ct = default)
        => await Db.Trips.Where(t => t.Status == status).AsNoTracking().ToListAsync(ct);
}

public class MaintenanceLogRepository(FleetDbContext db) : Repository<MaintenanceLog>(db), IMaintenanceLogRepository
{
    public async Task<IReadOnlyList<MaintenanceLog>> GetByVehicleAsync(int vehicleId, CancellationToken ct = default)
        => await Db.MaintenanceLogs.Where(m => m.VehicleId == vehicleId).OrderByDescending(m => m.MaintenanceDate).AsNoTracking().ToListAsync(ct);
}

public class FuelLogRepository(FleetDbContext db) : Repository<FuelLog>(db), IFuelLogRepository
{
    public async Task<IReadOnlyList<FuelLog>> GetByVehicleAsync(int vehicleId, CancellationToken ct = default)
        => await Db.FuelLogs.Where(f => f.VehicleId == vehicleId).OrderByDescending(f => f.FuelDate).AsNoTracking().ToListAsync(ct);
}
