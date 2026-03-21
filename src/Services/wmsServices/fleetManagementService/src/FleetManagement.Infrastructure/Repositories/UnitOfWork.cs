using FleetManagement.Domain.Interfaces;
using FleetManagement.Infrastructure.Data;

namespace FleetManagement.Infrastructure.Repositories;

public class UnitOfWork(FleetDbContext db) : IUnitOfWork
{
    private IVehicleRepository? _vehicles;
    private IDriverRepository? _drivers;
    private IRouteRepository? _routes;
    private ITripRepository? _trips;
    private IMaintenanceLogRepository? _maintenanceLogs;
    private IFuelLogRepository? _fuelLogs;

    public IVehicleRepository Vehicles => _vehicles ??= new VehicleRepository(db);
    public IDriverRepository Drivers => _drivers ??= new DriverRepository(db);
    public IRouteRepository Routes => _routes ??= new RouteRepository(db);
    public ITripRepository Trips => _trips ??= new TripRepository(db);
    public IMaintenanceLogRepository MaintenanceLogs => _maintenanceLogs ??= new MaintenanceLogRepository(db);
    public IFuelLogRepository FuelLogs => _fuelLogs ??= new FuelLogRepository(db);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await db.SaveChangesAsync(ct);

    public void Dispose() => db.Dispose();
}
