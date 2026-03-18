namespace AttendanceService.Domain.Interfaces;

public interface IUnitOfWork
{
    ISwipePunchRepository SwipePunches { get; }
    IAttendanceBatchRepository AttendanceBatches { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
