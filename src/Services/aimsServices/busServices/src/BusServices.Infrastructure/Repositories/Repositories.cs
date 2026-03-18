using BusServices.Domain.Entities;
using BusServices.Domain.Interfaces;
using BusServices.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BusServices.Infrastructure.Repositories;

public sealed class BusRepository : IBusRepository
{
    private readonly BusDbContext _ctx;

    public BusRepository(BusDbContext ctx) => _ctx = ctx;

    public Task<Bus?> GetByIdAsync(int busId, CancellationToken ct = default)
        => _ctx.Buses
            .Include(b => b.Routes)
            .Include(b => b.Arrivals)
            .Include(b => b.DeductionRates)
            .FirstOrDefaultAsync(b => b.BusId == busId, ct);

    public Task<Bus?> GetByRegistrationNumberAsync(string regNumber, CancellationToken ct = default)
        => _ctx.Buses.FirstOrDefaultAsync(b => b.RegistrationNumber.Value == regNumber, ct);

    public async Task<IEnumerable<Bus>> GetAllAsync(CancellationToken ct = default)
        => await _ctx.Buses.ToListAsync(ct);

    public Task<bool> ExistsAsync(int busId, CancellationToken ct = default)
        => _ctx.Buses.AnyAsync(b => b.BusId == busId, ct);

    public Task<bool> RegistrationExistsAsync(string regNumber, CancellationToken ct = default)
        => _ctx.Buses.AnyAsync(b => b.RegistrationNumber.Value == regNumber, ct);

    public async Task<int> GetNextIdAsync(CancellationToken ct = default)
        => (await _ctx.Buses.MaxAsync(b => (int?)b.BusId, ct) ?? 0) + 1;

    public async Task AddAsync(Bus bus, CancellationToken ct = default)
        => await _ctx.Buses.AddAsync(bus, ct);

    public void Update(Bus bus)
        => _ctx.Buses.Update(bus);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _ctx.SaveChangesAsync(ct);
}

public sealed class BusRouteRepository : IBusRouteRepository
{
    private readonly BusDbContext _ctx;

    public BusRouteRepository(BusDbContext ctx) => _ctx = ctx;

    public Task<BusRoute?> GetByIdAsync(int routeId, CancellationToken ct = default)
        => _ctx.BusRoutes.FirstOrDefaultAsync(r => r.RouteId == routeId, ct);

    public async Task<IEnumerable<BusRoute>> GetByBusIdAsync(int busId, CancellationToken ct = default)
        => await _ctx.BusRoutes.Where(r => r.BusId == busId).ToListAsync(ct);

    public Task<bool> ExistsForBusAsync(int routeId, int busId, CancellationToken ct = default)
        => _ctx.BusRoutes.AnyAsync(r => r.RouteId == routeId && r.BusId == busId, ct);

    public async Task<int> GetNextIdAsync(CancellationToken ct = default)
        => (await _ctx.BusRoutes.MaxAsync(r => (int?)r.RouteId, ct) ?? 0) + 1;

    public async Task AddAsync(BusRoute route, CancellationToken ct = default)
        => await _ctx.BusRoutes.AddAsync(route, ct);

    public void Update(BusRoute route)
        => _ctx.BusRoutes.Update(route);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _ctx.SaveChangesAsync(ct);
}

public sealed class EmployeeBusRepository : IEmployeeBusRepository
{
    private readonly BusDbContext _ctx;

    public EmployeeBusRepository(BusDbContext ctx) => _ctx = ctx;

    public Task<EmployeeBus?> GetByIdAsync(long empBusId, CancellationToken ct = default)
        => _ctx.EmployeeBusAssignments.FirstOrDefaultAsync(e => e.EmpBusId == empBusId, ct);

    public async Task<IEnumerable<EmployeeBus>> GetByEmployeeIdAsync(long empSysId, CancellationToken ct = default)
        => await _ctx.EmployeeBusAssignments.Where(e => e.EmpSysId == empSysId).ToListAsync(ct);

    public async Task<IEnumerable<EmployeeBus>> GetByBusIdAsync(int busId, CancellationToken ct = default)
        => await _ctx.EmployeeBusAssignments.Where(e => e.BusId == busId).ToListAsync(ct);

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
        => (await _ctx.EmployeeBusAssignments.MaxAsync(e => (long?)e.EmpBusId, ct) ?? 0) + 1;

    public async Task AddAsync(EmployeeBus assignment, CancellationToken ct = default)
        => await _ctx.EmployeeBusAssignments.AddAsync(assignment, ct);

    public void Update(EmployeeBus assignment)
        => _ctx.EmployeeBusAssignments.Update(assignment);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _ctx.SaveChangesAsync(ct);
}

public sealed class BusArrivalRepository : IBusArrivalRepository
{
    private readonly BusDbContext _ctx;

    public BusArrivalRepository(BusDbContext ctx) => _ctx = ctx;

    public Task<BusArrival?> GetByIdAsync(long arrivalId, CancellationToken ct = default)
        => _ctx.BusArrivals.FirstOrDefaultAsync(a => a.ArrivalId == arrivalId, ct);

    public async Task<IEnumerable<BusArrival>> GetByBusIdAsync(int busId, CancellationToken ct = default)
        => await _ctx.BusArrivals.Where(a => a.BusId == busId).ToListAsync(ct);

    public async Task<IEnumerable<BusArrival>> GetByDateAsync(DateTime date, CancellationToken ct = default)
        => await _ctx.BusArrivals.Where(a => a.ArrivalDate.Date == date.Date).ToListAsync(ct);

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
        => (await _ctx.BusArrivals.MaxAsync(a => (long?)a.ArrivalId, ct) ?? 0) + 1;

    public async Task AddAsync(BusArrival arrival, CancellationToken ct = default)
        => await _ctx.BusArrivals.AddAsync(arrival, ct);

    public void Update(BusArrival arrival)
        => _ctx.BusArrivals.Update(arrival);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _ctx.SaveChangesAsync(ct);
}

public sealed class BusDeductionRateRepository : IBusDeductionRateRepository
{
    private readonly BusDbContext _ctx;

    public BusDeductionRateRepository(BusDbContext ctx) => _ctx = ctx;

    public Task<BusDeductionRate?> GetByIdAsync(int deductId, CancellationToken ct = default)
        => _ctx.BusDeductionRates.FirstOrDefaultAsync(d => d.DeductId == deductId, ct);

    public async Task<IEnumerable<BusDeductionRate>> GetByBusIdAsync(int busId, CancellationToken ct = default)
        => await _ctx.BusDeductionRates.Where(d => d.BusId == busId).ToListAsync(ct);

    public Task<BusDeductionRate?> GetActiveRateAsync(int busId, DateTime onDate, CancellationToken ct = default)
        => _ctx.BusDeductionRates
            .Where(d => d.BusId == busId && d.EffectiveDate <= onDate && (d.ClosingDate == null || d.ClosingDate >= onDate))
            .OrderByDescending(d => d.EffectiveDate)
            .FirstOrDefaultAsync(ct);

    public async Task<int> GetNextIdAsync(CancellationToken ct = default)
        => (await _ctx.BusDeductionRates.MaxAsync(d => (int?)d.DeductId, ct) ?? 0) + 1;

    public async Task AddAsync(BusDeductionRate rate, CancellationToken ct = default)
        => await _ctx.BusDeductionRates.AddAsync(rate, ct);

    public void Update(BusDeductionRate rate)
        => _ctx.BusDeductionRates.Update(rate);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _ctx.SaveChangesAsync(ct);
}
