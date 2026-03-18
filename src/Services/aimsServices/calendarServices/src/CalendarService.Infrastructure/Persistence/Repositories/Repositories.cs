using CalendarService.Domain.Entities;
using CalendarService.Domain.Interfaces;
using CalendarService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CalendarService.Infrastructure.Persistence.Repositories;

public class CalendarRepository(CalendarDbContext db) : ICalendarRepository
{
    public Task<CalendarMaster?> GetByIdAsync(int id, CancellationToken ct)
        => db.CalendarMasters.Include(c => c.UnitMaps).Include(c => c.RoundRanges)
              .Include(c => c.GraceRanges).FirstOrDefaultAsync(c => c.CalendarId == id, ct);

    public Task<IEnumerable<CalendarMaster>> GetAllAsync(CancellationToken ct)
        => Task.FromResult<IEnumerable<CalendarMaster>>(db.CalendarMasters.AsNoTracking());

    public Task<bool> ExistsByNameAsync(string name, CancellationToken ct)
        => db.CalendarMasters.AnyAsync(c => c.CalendarName == name, ct);

    public async Task AddAsync(CalendarMaster entity, CancellationToken ct)
    {
        await db.CalendarMasters.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(CalendarMaster entity, CancellationToken ct)
    {
        db.CalendarMasters.Update(entity);
        await db.SaveChangesAsync(ct);
    }

    public async Task<int> GetNextIdAsync(CancellationToken ct)
    {
        var max = await db.CalendarMasters.MaxAsync(c => (int?)c.CalendarId, ct) ?? 0;
        return max + 1;
    }
}

public class HolidayRepository(CalendarDbContext db) : IHolidayRepository
{
    public Task<HolidayMaster?> GetByIdAsync(int id, CancellationToken ct)
        => db.HolidayMasters.FirstOrDefaultAsync(h => h.HolidayId == id, ct);

    public Task<IEnumerable<HolidayMaster>> GetAllAsync(CancellationToken ct)
        => Task.FromResult<IEnumerable<HolidayMaster>>(db.HolidayMasters.AsNoTracking());

    public async Task<IEnumerable<HolidayMaster>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct)
        => await db.HolidayMasters.Where(h => h.HolidayDate >= from && h.HolidayDate <= to).ToListAsync(ct);

    public async Task AddAsync(HolidayMaster entity, CancellationToken ct)
    {
        await db.HolidayMasters.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(HolidayMaster entity, CancellationToken ct)
    {
        db.HolidayMasters.Update(entity);
        await db.SaveChangesAsync(ct);
    }

    public async Task<int> GetNextIdAsync(CancellationToken ct)
    {
        var max = await db.HolidayMasters.MaxAsync(h => (int?)h.HolidayId, ct) ?? 0;
        return max + 1;
    }
}

public class ShiftRepository(CalendarDbContext db) : IShiftRepository
{
    public Task<ShiftMaster?> GetByIdAsync(int id, CancellationToken ct)
        => db.ShiftMasters.Include(s => s.TimeMasters).Include(s => s.Exceptions)
              .FirstOrDefaultAsync(s => s.ShiftId == id, ct);

    public Task<IEnumerable<ShiftMaster>> GetAllAsync(CancellationToken ct)
        => Task.FromResult<IEnumerable<ShiftMaster>>(db.ShiftMasters.AsNoTracking());

    public Task<bool> ExistsByCodeAsync(string code, CancellationToken ct)
        => db.ShiftMasters.AnyAsync(s => s.ShiftCode == code, ct);

    public async Task AddAsync(ShiftMaster entity, CancellationToken ct)
    {
        await db.ShiftMasters.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(ShiftMaster entity, CancellationToken ct)
    {
        db.ShiftMasters.Update(entity);
        await db.SaveChangesAsync(ct);
    }

    public async Task<int> GetNextIdAsync(CancellationToken ct)
    {
        var max = await db.ShiftMasters.MaxAsync(s => (int?)s.ShiftId, ct) ?? 0;
        return max + 1;
    }
}

public class PatternRepository(CalendarDbContext db) : IPatternRepository
{
    public Task<PatternMaster?> GetByIdAsync(int id, CancellationToken ct)
        => db.PatternMasters.Include(p => p.Details).ThenInclude(d => d.Shift)
              .FirstOrDefaultAsync(p => p.PatternId == id, ct);

    public Task<IEnumerable<PatternMaster>> GetAllAsync(CancellationToken ct)
        => Task.FromResult<IEnumerable<PatternMaster>>(db.PatternMasters.AsNoTracking());

    public async Task AddAsync(PatternMaster entity, CancellationToken ct)
    {
        await db.PatternMasters.AddAsync(entity, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(PatternMaster entity, CancellationToken ct)
    {
        db.PatternMasters.Update(entity);
        await db.SaveChangesAsync(ct);
    }

    public async Task<int> GetNextIdAsync(CancellationToken ct)
    {
        var max = await db.PatternMasters.MaxAsync(p => (int?)p.PatternId, ct) ?? 0;
        return max + 1;
    }
}
