using TimeSheetService.Domain.Entities;

namespace TimeSheetService.Domain.Interfaces;

public interface ITimesheetRepository
{
    Task<TimesheetEntry?> GetByIdAsync(long timeId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TimesheetEntry>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TimesheetEntry>> GetByEmployeeAsync(long employeeSysId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TimesheetEntry>> GetByDateRangeAsync(long employeeSysId, DateTime from, DateTime to, CancellationToken cancellationToken = default);
    Task<TimesheetEntry> AddAsync(TimesheetEntry entry, CancellationToken cancellationToken = default);
    Task UpdateAsync(TimesheetEntry entry, CancellationToken cancellationToken = default);
    Task DeleteAsync(long timeId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long timeId, CancellationToken cancellationToken = default);
}
