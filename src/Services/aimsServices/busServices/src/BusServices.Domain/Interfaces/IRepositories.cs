using BusServices.Domain.Entities;

namespace BusServices.Domain.Interfaces;

public interface IBusRepository
{
    Task<Bus?> GetByIdAsync(int busId, CancellationToken ct = default);
    Task<Bus?> GetByRegistrationNumberAsync(string regNumber, CancellationToken ct = default);
    Task<IEnumerable<Bus>> GetAllAsync(CancellationToken ct = default);
    Task<bool> ExistsAsync(int busId, CancellationToken ct = default);
    Task<bool> RegistrationExistsAsync(string regNumber, CancellationToken ct = default);
    Task<int> GetNextIdAsync(CancellationToken ct = default);
    Task AddAsync(Bus bus, CancellationToken ct = default);
    void Update(Bus bus);
    Task SaveChangesAsync(CancellationToken ct = default);
}

public interface IBusRouteRepository
{
    Task<BusRoute?> GetByIdAsync(int routeId, CancellationToken ct = default);
    Task<IEnumerable<BusRoute>> GetByBusIdAsync(int busId, CancellationToken ct = default);
    Task<bool> ExistsForBusAsync(int routeId, int busId, CancellationToken ct = default);
    Task<int> GetNextIdAsync(CancellationToken ct = default);
    Task AddAsync(BusRoute route, CancellationToken ct = default);
    void Update(BusRoute route);
    Task SaveChangesAsync(CancellationToken ct = default);
}

public interface IEmployeeBusRepository
{
    Task<EmployeeBus?> GetByIdAsync(long empBusId, CancellationToken ct = default);
    Task<IEnumerable<EmployeeBus>> GetByEmployeeIdAsync(long empSysId, CancellationToken ct = default);
    Task<IEnumerable<EmployeeBus>> GetByBusIdAsync(int busId, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
    Task AddAsync(EmployeeBus assignment, CancellationToken ct = default);
    void Update(EmployeeBus assignment);
    Task SaveChangesAsync(CancellationToken ct = default);
}

public interface IBusArrivalRepository
{
    Task<BusArrival?> GetByIdAsync(long arrivalId, CancellationToken ct = default);
    Task<IEnumerable<BusArrival>> GetByBusIdAsync(int busId, CancellationToken ct = default);
    Task<IEnumerable<BusArrival>> GetByDateAsync(DateTime date, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
    Task AddAsync(BusArrival arrival, CancellationToken ct = default);
    void Update(BusArrival arrival);
    Task SaveChangesAsync(CancellationToken ct = default);
}

public interface IBusDeductionRateRepository
{
    Task<BusDeductionRate?> GetByIdAsync(int deductId, CancellationToken ct = default);
    Task<IEnumerable<BusDeductionRate>> GetByBusIdAsync(int busId, CancellationToken ct = default);
    Task<BusDeductionRate?> GetActiveRateAsync(int busId, DateTime onDate, CancellationToken ct = default);
    Task<int> GetNextIdAsync(CancellationToken ct = default);
    Task AddAsync(BusDeductionRate rate, CancellationToken ct = default);
    void Update(BusDeductionRate rate);
    Task SaveChangesAsync(CancellationToken ct = default);
}
