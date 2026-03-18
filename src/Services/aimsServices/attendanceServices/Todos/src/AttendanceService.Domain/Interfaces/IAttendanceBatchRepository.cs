using AttendanceService.Domain.Entities;

namespace AttendanceService.Domain.Interfaces;

public interface IAttendanceBatchRepository : IRepository<AttendanceBatch>
{
    Task<IEnumerable<AttendanceBatch>> GetByStatusAsync(string status, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}
