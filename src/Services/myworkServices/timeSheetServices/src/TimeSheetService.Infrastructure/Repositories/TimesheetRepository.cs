using Microsoft.EntityFrameworkCore;
using TimeSheetService.Domain.Entities;
using TimeSheetService.Domain.Interfaces;
using TimeSheetService.Infrastructure.Persistence;

namespace TimeSheetService.Infrastructure.Repositories;

public class TimesheetRepository : ITimesheetRepository
{
    private readonly TimeSheetDbContext _context;

    public TimesheetRepository(TimeSheetDbContext context) => _context = context;

    public async Task<TimesheetEntry?> GetByIdAsync(long timeId, CancellationToken cancellationToken = default)
        => await _context.TimesheetEntries
            .Include(e => e.Details)
            .FirstOrDefaultAsync(e => e.Id == timeId, cancellationToken);

    public async Task<IReadOnlyList<TimesheetEntry>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.TimesheetEntries
            .Include(e => e.Details)
            .OrderByDescending(e => e.TimeDate)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TimesheetEntry>> GetByEmployeeAsync(long employeeSysId, CancellationToken cancellationToken = default)
        => await _context.TimesheetEntries
            .Include(e => e.Details)
            .Where(e => e.EmployeeSysId == employeeSysId)
            .OrderByDescending(e => e.TimeDate)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TimesheetEntry>> GetByDateRangeAsync(long employeeSysId, DateTime from, DateTime to, CancellationToken cancellationToken = default)
        => await _context.TimesheetEntries
            .Include(e => e.Details)
            .Where(e => e.EmployeeSysId == employeeSysId && e.TimeDate >= from && e.TimeDate <= to)
            .OrderByDescending(e => e.TimeDate)
            .ToListAsync(cancellationToken);

    public async Task<TimesheetEntry> AddAsync(TimesheetEntry entry, CancellationToken cancellationToken = default)
    {
        await _context.TimesheetEntries.AddAsync(entry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entry;
    }

    public async Task UpdateAsync(TimesheetEntry entry, CancellationToken cancellationToken = default)
    {
        _context.TimesheetEntries.Update(entry);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long timeId, CancellationToken cancellationToken = default)
    {
        var entry = await GetByIdAsync(timeId, cancellationToken);
        if (entry is not null)
        {
            _context.TimesheetEntries.Remove(entry);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<bool> ExistsAsync(long timeId, CancellationToken cancellationToken = default)
        => await _context.TimesheetEntries.AnyAsync(e => e.Id == timeId, cancellationToken);
}
