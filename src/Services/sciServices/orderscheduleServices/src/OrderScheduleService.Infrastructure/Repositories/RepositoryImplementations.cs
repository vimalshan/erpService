namespace OrderScheduleService.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using OrderScheduleService.Domain.Aggregates;
using OrderScheduleService.Domain.Interfaces;
using OrderScheduleService.Infrastructure.Persistence;

public class TiedOrderRepository : ITiedOrderRepository
{
    private readonly OrderScheduleDbContext _context;

    public TiedOrderRepository(OrderScheduleDbContext context)
    {
        _context = context;
    }

    public async Task<TiedOrderAggregate?> GetByIdAsync(long id)
    {
        return await _context.TiedOrders
            .Include(o => o.Details)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<TiedOrderAggregate>> GetByCustomerAsync(string customerCode)
    {
        return await _context.TiedOrders
            .Include(o => o.Details)
            .Where(o => o.CustomerCode == customerCode)
            .ToListAsync();
    }

    public async Task<IEnumerable<TiedOrderAggregate>> GetAllAsync()
    {
        return await _context.TiedOrders
            .Include(o => o.Details)
            .ToListAsync();
    }

    public async Task AddAsync(TiedOrderAggregate order)
    {
        await _context.TiedOrders.AddAsync(order);
    }

    public async Task UpdateAsync(TiedOrderAggregate order)
    {
        _context.TiedOrders.Update(order);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(long id)
    {
        var order = await _context.TiedOrders.FirstOrDefaultAsync(o => o.Id == id);
        if (order != null)
        {
            _context.TiedOrders.Remove(order);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

public class ScheduleRepository : IScheduleRepository
{
    private readonly OrderScheduleDbContext _context;

    public ScheduleRepository(OrderScheduleDbContext context)
    {
        _context = context;
    }

    public async Task<ScheduleAggregate?> GetByIdAsync(long id)
    {
        return await _context.Schedules
            .Include(s => s.ScheduleDetails)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<ScheduleAggregate>> GetByItemAsync(decimal itemId)
    {
        return await _context.Schedules
            .Include(s => s.ScheduleDetails)
            .Where(s => s.ItemId == itemId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ScheduleAggregate>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate)
    {
        return await _context.Schedules
            .Include(s => s.ScheduleDetails)
            .Where(s => s.RequiredDate >= fromDate && s.RequiredDate <= toDate)
            .ToListAsync();
    }

    public async Task AddAsync(ScheduleAggregate schedule)
    {
        await _context.Schedules.AddAsync(schedule);
    }

    public async Task UpdateAsync(ScheduleAggregate schedule)
    {
        _context.Schedules.Update(schedule);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(long id)
    {
        var schedule = await _context.Schedules.FirstOrDefaultAsync(s => s.Id == id);
        if (schedule != null)
        {
            _context.Schedules.Remove(schedule);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

public class ShiftRepository : IShiftRepository
{
    private readonly OrderScheduleDbContext _context;

    public ShiftRepository(OrderScheduleDbContext context)
    {
        _context = context;
    }

    public async Task<OrderScheduleService.Domain.Entities.Shift?> GetByIdAsync(char shiftCode, decimal companyUnitId)
    {
        return await _context.Shifts
            .FirstOrDefaultAsync(s => s.ShiftCode == shiftCode && s.CompanyUnitId == companyUnitId);
    }

    public async Task<IEnumerable<OrderScheduleService.Domain.Entities.Shift>> GetByCompanyAsync(decimal companyUnitId)
    {
        return await _context.Shifts
            .Where(s => s.CompanyUnitId == companyUnitId)
            .ToListAsync();
    }

    public async Task<IEnumerable<OrderScheduleService.Domain.Entities.Shift>> GetAllAsync()
    {
        return await _context.Shifts.ToListAsync();
    }

    public async Task AddAsync(OrderScheduleService.Domain.Entities.Shift shift)
    {
        await _context.Shifts.AddAsync(shift);
    }

    public async Task UpdateAsync(OrderScheduleService.Domain.Entities.Shift shift)
    {
        _context.Shifts.Update(shift);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(char shiftCode, decimal companyUnitId)
    {
        var shift = await _context.Shifts
            .FirstOrDefaultAsync(s => s.ShiftCode == shiftCode && s.CompanyUnitId == companyUnitId);
        if (shift != null)
        {
            _context.Shifts.Remove(shift);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
