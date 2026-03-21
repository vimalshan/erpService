using FleetManagement.Domain.Common;

namespace FleetManagement.Domain.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<T>> GetAllAsync(CancellationToken ct = default);
    Task<T> AddAsync(T entity, CancellationToken ct = default);
    Task UpdateAsync(T entity, CancellationToken ct = default);
    Task DeleteAsync(T entity, CancellationToken ct = default);
}

public interface IVehicleRepository : IRepository<Entities.Vehicle>
{
    Task<Entities.Vehicle?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<IReadOnlyList<Entities.Vehicle>> GetByStatusAsync(Enums.VehicleStatus status, CancellationToken ct = default);
    Task<IReadOnlyList<Entities.Vehicle>> GetByWarehouseAsync(int warehouseId, CancellationToken ct = default);
}

public interface IDriverRepository : IRepository<Entities.Driver>
{
    Task<Entities.Driver?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<IReadOnlyList<Entities.Driver>> GetActiveDriversAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Entities.Driver>> GetDriversWithExpiringLicensesAsync(int daysThreshold, CancellationToken ct = default);
}

public interface IRouteRepository : IRepository<Entities.Route>
{
    Task<Entities.Route?> GetByNameAsync(string routeName, CancellationToken ct = default);
}

public interface ITripRepository : IRepository<Entities.Trip>
{
    Task<Entities.Trip?> GetByTripNumberAsync(string tripNumber, CancellationToken ct = default);
    Task<Entities.Trip?> GetWithStopsAsync(int tripId, CancellationToken ct = default);
    Task<IReadOnlyList<Entities.Trip>> GetByVehicleAsync(int vehicleId, CancellationToken ct = default);
    Task<IReadOnlyList<Entities.Trip>> GetByDriverAsync(int driverId, CancellationToken ct = default);
    Task<IReadOnlyList<Entities.Trip>> GetByStatusAsync(Enums.TripStatus status, CancellationToken ct = default);
}

public interface IMaintenanceLogRepository : IRepository<Entities.MaintenanceLog>
{
    Task<IReadOnlyList<Entities.MaintenanceLog>> GetByVehicleAsync(int vehicleId, CancellationToken ct = default);
}

public interface IFuelLogRepository : IRepository<Entities.FuelLog>
{
    Task<IReadOnlyList<Entities.FuelLog>> GetByVehicleAsync(int vehicleId, CancellationToken ct = default);
}

public interface IUnitOfWork : IDisposable
{
    IVehicleRepository Vehicles { get; }
    IDriverRepository Drivers { get; }
    IRouteRepository Routes { get; }
    ITripRepository Trips { get; }
    IMaintenanceLogRepository MaintenanceLogs { get; }
    IFuelLogRepository FuelLogs { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
