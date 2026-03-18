using TimesheetService.Domain.Entities;

namespace TimesheetService.Domain.Interfaces;

public interface ITimesheetRepository
{
    Task<Timesheet?> GetByIdAsync(long timesheetId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Timesheet>> GetByEmployeeIdAsync(long employeeId, DateOnly? from = null, DateOnly? to = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<Timesheet>> GetPendingTimesheetsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Timesheet>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<Timesheet> AddAsync(Timesheet timesheet, CancellationToken cancellationToken = default);
    Task UpdateAsync(Timesheet timesheet, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long timesheetId, CancellationToken cancellationToken = default);
    Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);
}
