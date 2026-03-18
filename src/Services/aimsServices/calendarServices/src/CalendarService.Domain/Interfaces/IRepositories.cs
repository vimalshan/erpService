using CalendarService.Domain.Entities;

namespace CalendarService.Domain.Interfaces;

public interface ICalendarRepository
{
    Task<CalendarMaster?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<CalendarMaster>> GetAllAsync(CancellationToken ct = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default);
    Task AddAsync(CalendarMaster entity, CancellationToken ct = default);
    Task UpdateAsync(CalendarMaster entity, CancellationToken ct = default);
    Task<int> GetNextIdAsync(CancellationToken ct = default);
}

public interface IHolidayRepository
{
    Task<HolidayMaster?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<HolidayMaster>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<HolidayMaster>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task AddAsync(HolidayMaster entity, CancellationToken ct = default);
    Task UpdateAsync(HolidayMaster entity, CancellationToken ct = default);
    Task<int> GetNextIdAsync(CancellationToken ct = default);
}

public interface IShiftRepository
{
    Task<ShiftMaster?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<ShiftMaster>> GetAllAsync(CancellationToken ct = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default);
    Task AddAsync(ShiftMaster entity, CancellationToken ct = default);
    Task UpdateAsync(ShiftMaster entity, CancellationToken ct = default);
    Task<int> GetNextIdAsync(CancellationToken ct = default);
}

public interface IPatternRepository
{
    Task<PatternMaster?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<PatternMaster>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(PatternMaster entity, CancellationToken ct = default);
    Task UpdateAsync(PatternMaster entity, CancellationToken ct = default);
    Task<int> GetNextIdAsync(CancellationToken ct = default);
}
