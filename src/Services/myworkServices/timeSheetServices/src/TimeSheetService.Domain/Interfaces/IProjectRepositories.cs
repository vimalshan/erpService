using TimeSheetService.Domain.Entities;

namespace TimeSheetService.Domain.Interfaces;

public interface ITcTimesheetRepository
{
    Task<TcTimesheetEntry?> GetByIdAsync(long timeId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TcTimesheetEntry>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TcTimesheetEntry>> GetByEmployeeAsync(long employeeSysId, CancellationToken cancellationToken = default);
    Task<TcTimesheetEntry> AddAsync(TcTimesheetEntry entry, CancellationToken cancellationToken = default);
    Task UpdateAsync(TcTimesheetEntry entry, CancellationToken cancellationToken = default);
    Task DeleteAsync(long timeId, CancellationToken cancellationToken = default);
}

public interface ITcProjectRepository
{
    Task<TcProject?> GetByIdAsync(long projectId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TcProject>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TcProject> AddAsync(TcProject project, CancellationToken cancellationToken = default);
    Task UpdateAsync(TcProject project, CancellationToken cancellationToken = default);
}

public interface ITsProjectRepository
{
    Task<TsProject?> GetByCodeAsync(string projectCode, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TsProject>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TsProject> AddAsync(TsProject project, CancellationToken cancellationToken = default);
}
