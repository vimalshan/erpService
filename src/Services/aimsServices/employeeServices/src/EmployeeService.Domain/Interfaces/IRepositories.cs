using EmployeeService.Domain.Entities;

namespace EmployeeService.Domain.Interfaces;

public interface IEmployeeTimeInfoRepository
{
    Task<EmployeeTimeInfo?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<EmployeeTimeInfo>> GetByEmployeeIdAsync(long empSysId, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
    Task AddAsync(EmployeeTimeInfo entity, CancellationToken ct = default);
    Task UpdateAsync(EmployeeTimeInfo entity, CancellationToken ct = default);
}

public interface IEmployeeApproverRepository
{
    Task<EmployeeApprover?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<EmployeeApprover>> GetByEmployeeIdAsync(long empSysId, CancellationToken ct = default);
    Task<int> GetNextIdAsync(CancellationToken ct = default);
    Task AddAsync(EmployeeApprover entity, CancellationToken ct = default);
}

public interface IEmployeeCalendarRepository
{
    Task<EmployeeCalendar?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<EmployeeCalendar>> GetByEmployeeIdAsync(long empSysId, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
    Task AddAsync(EmployeeCalendar entity, CancellationToken ct = default);
    Task UpdateAsync(EmployeeCalendar entity, CancellationToken ct = default);
}

public interface IEmployeePatternRepository
{
    Task<EmployeePattern?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<EmployeePattern>> GetByEmployeeIdAsync(long empSysId, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
    Task AddAsync(EmployeePattern entity, CancellationToken ct = default);
}

public interface IEmployeeShiftRepository
{
    Task<EmployeeShift?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<EmployeeShift>> GetByEmployeeIdAsync(long empSysId, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
    Task AddAsync(EmployeeShift entity, CancellationToken ct = default);
}

public interface IEmployeeShiftPatternRepository
{
    Task<EmployeeShiftPattern?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<EmployeeShiftPattern>> GetByEmployeeIdAsync(long empSysId, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
    Task AddAsync(EmployeeShiftPattern entity, CancellationToken ct = default);
}
