using Microsoft.EntityFrameworkCore;
using TimeSheetService.Domain.Entities;
using TimeSheetService.Domain.Interfaces;
using TimeSheetService.Infrastructure.Persistence;

namespace TimeSheetService.Infrastructure.Repositories;

public class TcTimesheetRepository : ITcTimesheetRepository
{
    private readonly TimeSheetDbContext _context;

    public TcTimesheetRepository(TimeSheetDbContext context) => _context = context;

    public async Task<TcTimesheetEntry?> GetByIdAsync(long timeId, CancellationToken cancellationToken = default)
        => await _context.TcTimesheetEntries
            .Include(e => e.Details)
            .FirstOrDefaultAsync(e => e.Id == timeId, cancellationToken);

    public async Task<IReadOnlyList<TcTimesheetEntry>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.TcTimesheetEntries
            .Include(e => e.Details)
            .OrderByDescending(e => e.TimeDate)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TcTimesheetEntry>> GetByEmployeeAsync(long employeeSysId, CancellationToken cancellationToken = default)
        => await _context.TcTimesheetEntries
            .Include(e => e.Details)
            .Where(e => e.EmployeeSysId == employeeSysId)
            .OrderByDescending(e => e.TimeDate)
            .ToListAsync(cancellationToken);

    public async Task<TcTimesheetEntry> AddAsync(TcTimesheetEntry entry, CancellationToken cancellationToken = default)
    {
        await _context.TcTimesheetEntries.AddAsync(entry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entry;
    }

    public async Task UpdateAsync(TcTimesheetEntry entry, CancellationToken cancellationToken = default)
    {
        _context.TcTimesheetEntries.Update(entry);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(long timeId, CancellationToken cancellationToken = default)
    {
        var entry = await GetByIdAsync(timeId, cancellationToken);
        if (entry is not null)
        {
            _context.TcTimesheetEntries.Remove(entry);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
