using AttendanceService.Domain.Entities;

namespace AttendanceService.Domain.Interfaces;

public interface ISwipePunchRepository : IRepository<SwipeRawPunch>
{
    Task<IEnumerable<SwipeRawPunch>> GetByEmployeeAsync(long empSysId, CancellationToken ct = default);
    Task<IEnumerable<SwipeRawPunch>> GetByEmployeeAndDateRangeAsync(long empSysId,
        DateTime from, DateTime to, CancellationToken ct = default);
    Task<int> GetDistinctPunchDaysAsync(long empSysId, DateTime from, DateTime to, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}
